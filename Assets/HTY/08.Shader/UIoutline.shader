Shader "Custom/UIoutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}

        _Color ("Tint", Color) = (1,1,1,1)

        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineSize ("Outline Size", Range(0, 5)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;

            float4 _MainTex_TexelSize;

            fixed4 _Color;
            fixed4 _OutlineColor;

            float _OutlineSize;

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 original =
                    tex2D(_MainTex, i.uv) * i.color;

                float2 offset =
                    _MainTex_TexelSize.xy * _OutlineSize;

                float aroundAlpha = 0;

                aroundAlpha = max(
                    aroundAlpha,
                    tex2D(_MainTex, i.uv + float2(offset.x, 0)).a
                );

                aroundAlpha = max(
                    aroundAlpha,
                    tex2D(_MainTex, i.uv + float2(-offset.x, 0)).a
                );

                aroundAlpha = max(
                    aroundAlpha,
                    tex2D(_MainTex, i.uv + float2(0, offset.y)).a
                );

                aroundAlpha = max(
                    aroundAlpha,
                    tex2D(_MainTex, i.uv + float2(0, -offset.y)).a
                );

                aroundAlpha = max(
                    aroundAlpha,
                    tex2D(_MainTex, i.uv + float2(offset.x, offset.y)).a
                );

                aroundAlpha = max(
                    aroundAlpha,
                    tex2D(_MainTex, i.uv + float2(-offset.x, offset.y)).a
                );

                aroundAlpha = max(
                    aroundAlpha,
                    tex2D(_MainTex, i.uv + float2(offset.x, -offset.y)).a
                );

                aroundAlpha = max(
                    aroundAlpha,
                    tex2D(_MainTex, i.uv + float2(-offset.x, -offset.y)).a
                );

                float outlineAlpha =
                    aroundAlpha * (1 - original.a);

                fixed4 result;

                result.rgb = lerp(
                    _OutlineColor.rgb,
                    original.rgb,
                    original.a
                );

                result.a = max(
                    original.a,
                    outlineAlpha * _OutlineColor.a
                );

                return result;
            }

            ENDCG
        }
    }
}