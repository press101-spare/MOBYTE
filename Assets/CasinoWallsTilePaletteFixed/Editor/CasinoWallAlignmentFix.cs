using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class CasinoWallAlignmentFix
{
    public const string Root = "Assets/CasinoWallsTilePaletteFixed";
    public enum Kind { None, Top, Bottom, TopLeft, TopRight, BottomLeft, BottomRight, Cropped }
    public static readonly string[] Names = { "", "horizontal_top", "horizontal_bottom", "corner_top_left", "corner_top_right", "corner_bottom_left", "corner_bottom_right" };
    static readonly Dictionary<int, Kind> VisualCache = new Dictionary<int, Kind>();
    static Signature[] ReferenceSignatures;

    [MenuItem("Tools/Casino Walls/Fix Selected Tilemap Alignment")]
    static void FixSelected()
    {
        var map = Selection.activeGameObject ? Selection.activeGameObject.GetComponent<Tilemap>() : null;
        if (!map) { EditorUtility.DisplayDialog("벽 연결 맞춤", "Hierarchy에서 벽을 칠한 Tilemap을 선택한 뒤 실행해 주세요.", "확인"); return; }
        try { int changed = Fix(map, true); EditorUtility.DisplayDialog("벽 연결 맞춤", changed + "칸의 벽을 맞췄습니다. Ctrl+Z로 되돌릴 수 있습니다.", "확인"); }
        catch (Exception e) { Debug.LogException(e); EditorUtility.DisplayDialog("벽 연결 맞춤", e.Message, "확인"); }
    }

    public static int Fix(Tilemap map, bool recordUndo)
    {
        VisualCache.Clear(); ReferenceSignatures = null;
        var grid = map.layoutGrid;
        if (!grid || grid.cellLayout != GridLayout.CellLayout.Rectangle || grid.cellSwizzle != GridLayout.CellSwizzle.XYZ)
            throw new InvalidOperationException("이 수정 도구는 XY 직사각형 Grid용입니다.");
        var replacements = new Dictionary<Kind, Tile>();
        for (int k = 1; k <= 6; k++) {
            var tile = AssetDatabase.LoadAssetAtPath<Tile>(Root + "/Tiles/" + Names[k] + ".asset");
            if (!tile) throw new InvalidOperationException("수정 패키지의 Tiles 폴더가 필요합니다: " + Names[k]);
            replacements.Add((Kind)k, tile);
        }
        var original = new Dictionary<Vector3Int, Kind>();
        foreach (var position in map.cellBounds.allPositionsWithin) {
            Kind kind = Identify(map.GetSprite(position));
            if (kind == Kind.None) continue;
            var matrix = map.GetTransformMatrix(position);
            if (kind >= Kind.TopLeft && kind <= Kind.BottomRight) {
                var direction = matrix.MultiplyVector(new Vector3(kind == Kind.TopLeft || kind == Kind.BottomLeft ? -1 : 1, kind == Kind.TopLeft || kind == Kind.TopRight ? 1 : -1, 0));
                kind = direction.y >= 0 ? (direction.x < 0 ? Kind.TopLeft : Kind.TopRight) : (direction.x < 0 ? Kind.BottomLeft : Kind.BottomRight);
            } else if (kind != Kind.Cropped) {
                var direction = matrix.MultiplyVector(new Vector3(0, kind == Kind.Top ? 1 : -1, 0));
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) continue;
                kind = direction.y >= 0 ? Kind.Top : Kind.Bottom;
            }
            original[position] = kind;
        }
        if (recordUndo) Undo.RegisterCompleteObjectUndo(map, "Align casino wall tiles");
        var origin = grid.CellToLocal(Vector3Int.zero);
        float sx = (grid.CellToLocal(Vector3Int.right) - origin).magnitude;
        float sy = (grid.CellToLocal(Vector3Int.up) - origin).magnitude;
        var transform = Matrix4x4.Scale(new Vector3(sx, sy, 1));
        int changed = 0;
        foreach (var cell in original) {
            Kind kind = cell.Value;
            if (kind == Kind.Top || kind == Kind.Bottom || kind == Kind.Cropped) {
                Kind left = FindEnd(original, cell.Key, -1), right = FindEnd(original, cell.Key, 1);
                if (left != Kind.None && right != Kind.None && left != right) continue;
                if (left != Kind.None) kind = left;
                else if (right != Kind.None) kind = right;
                else if (kind == Kind.Cropped) kind = Kind.Top;
            }
            Tile replacement = replacements[kind];
            var old = map.GetTile(cell.Key) as Tile;
            if (old && old.colliderType != replacement.colliderType) replacement = WithCollider(replacement, old.colliderType);
            Color color = map.GetColor(cell.Key);
            map.SetTile(cell.Key, replacement);
            map.SetTileFlags(cell.Key, TileFlags.None);
            map.SetTransformMatrix(cell.Key, transform);
            map.SetColor(cell.Key, color);
            changed++;
        }
        map.RefreshAllTiles();
        if (recordUndo) EditorUtility.SetDirty(map);
        return changed;
    }

    static Kind FindEnd(Dictionary<Vector3Int, Kind> cells, Vector3Int cell, int direction)
    {
        for (int i = 0; i < cells.Count; i++) {
            cell.x += direction;
            if (!cells.TryGetValue(cell, out Kind kind)) return Kind.None;
            if (kind == Kind.Top || kind == Kind.Bottom || kind == Kind.Cropped) continue;
            if (direction < 0) return kind == Kind.TopLeft ? Kind.Top : kind == Kind.BottomLeft ? Kind.Bottom : Kind.None;
            return kind == Kind.TopRight ? Kind.Top : kind == Kind.BottomRight ? Kind.Bottom : Kind.None;
        }
        return Kind.None;
    }

    public static Kind Identify(Sprite sprite)
    {
        if (!sprite) return Kind.None;
        string name = Regex.Replace(sprite.name.ToLowerInvariant(), @"_\d+$", "");
        switch (name) {
            case "horizontal_top": case "north": case "wall_north_64": return Kind.Top;
            case "horizontal_bottom": case "south": return Kind.Bottom;
            case "corner_top_left": case "corner_nw": return Kind.TopLeft;
            case "corner_top_right": case "corner_ne": return Kind.TopRight;
            case "corner_bottom_left": case "corner_sw": return Kind.BottomLeft;
            case "corner_bottom_right": case "corner_se": return Kind.BottomRight;
            case "wall_strip_64x16": return Kind.Cropped;
            default: return IdentifyImage(sprite);
        }
    }

    // Sprite Editor can trim a sheet and rename each slice. Compare its visible
    // source pixels, not only the filename, so those legacy sprites are repairable.
    class Signature { public float aspect; public Color32[] pixels; }
    static Kind IdentifyImage(Sprite sprite)
    {
        if (VisualCache.TryGetValue(sprite.GetInstanceID(), out Kind cached)) return cached;
        Signature input = ReadSignature(sprite);
        Kind best = Kind.None; float bestError = 6;
        if (input != null) {
            if (ReferenceSignatures == null) {
                ReferenceSignatures = new Signature[7];
                for (int k = 1; k <= 6; k++) ReferenceSignatures[k] = ReadSignature(AssetDatabase.LoadAssetAtPath<Sprite>(Root + "/Sprites/" + Names[k] + ".png"));
            }
            for (int k = 1; k <= 6; k++) {
                Signature reference = ReferenceSignatures[k];
                if (reference == null || Mathf.Abs(input.aspect - reference.aspect) > 0.06f) continue;
                long error = 0; int samples = 0, alphaMismatch = 0;
                for (int i = 0; i < 4096; i++) {
                    var a = input.pixels[i]; var b = reference.pixels[i];
                    if ((a.a > 127) != (b.a > 127)) { alphaMismatch++; continue; }
                    if (a.a <= 127) continue;
                    error += Math.Abs(a.r - b.r) + Math.Abs(a.g - b.g) + Math.Abs(a.b - b.b); samples += 3;
                }
                float average = samples > 0 ? (float)error / samples : float.MaxValue;
                if (alphaMismatch < 24 && average < bestError) { bestError = average; best = (Kind)k; }
            }
        }
        VisualCache[sprite.GetInstanceID()] = best;
        return best;
    }
    static Signature ReadSignature(Sprite sprite)
    {
        if (!sprite) return null;
        string path = AssetDatabase.GetAssetPath(sprite);
        if (!File.Exists(path) || !string.Equals(Path.GetExtension(path), ".png", StringComparison.OrdinalIgnoreCase)) return null;
        var raw = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        try {
            if (!raw.LoadImage(File.ReadAllBytes(path))) return null;
            float sx = (float)raw.width / sprite.texture.width, sy = (float)raw.height / sprite.texture.height;
            Rect rect = sprite.rect;
            int x0 = Mathf.Clamp(Mathf.RoundToInt(rect.x * sx), 0, raw.width - 1), y0 = Mathf.Clamp(Mathf.RoundToInt(rect.y * sy), 0, raw.height - 1);
            int x1 = Mathf.Clamp(Mathf.RoundToInt(rect.xMax * sx), x0 + 1, raw.width), y1 = Mathf.Clamp(Mathf.RoundToInt(rect.yMax * sy), y0 + 1, raw.height);
            var colors = raw.GetPixels32(); int left = x1, right = x0 - 1, bottom = y1, top = y0 - 1;
            for (int y = y0; y < y1; y++) for (int x = x0; x < x1; x++) if (colors[y * raw.width + x].a > 127) { left = Math.Min(left, x); right = Math.Max(right, x); bottom = Math.Min(bottom, y); top = Math.Max(top, y); }
            if (right < left || top < bottom) return null;
            int width = right - left + 1, height = top - bottom + 1;
            var signature = new Signature { aspect = (float)width / height, pixels = new Color32[4096] };
            for (int y = 0; y < 64; y++) for (int x = 0; x < 64; x++) signature.pixels[y * 64 + x] = colors[(bottom + (int)((y + 0.5f) * height / 64)) * raw.width + left + (int)((x + 0.5f) * width / 64)];
            return signature;
        }
        finally { UnityEngine.Object.DestroyImmediate(raw); }
    }

    static Tile WithCollider(Tile tile, Tile.ColliderType collider)
    {
        string folder = Root + "/CollisionVariants";
        if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder(Root, "CollisionVariants");
        string path = folder + "/" + tile.name + "_" + collider + ".asset";
        var variant = AssetDatabase.LoadAssetAtPath<Tile>(path);
        if (!variant) { variant = UnityEngine.Object.Instantiate(tile); variant.name = tile.name; variant.colliderType = collider; AssetDatabase.CreateAsset(variant, path); }
        return variant;
    }
}
