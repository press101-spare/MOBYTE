using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceTree_JCY : MonoBehaviour
{
    public enum Trees
    {
        Choice,
        OnePair,
        TwoPair,
        Three_Of_AKind,
        Four_Of_AKind,
        FullHouse,
        SmallStraight,
        LargeStraight,
        Yahtzee
    }

    [SerializeField] private Transform treePanel;

    [System.Serializable]
    public struct TreeUI
    {
        public Trees treeType;
        public GameObject checkMarkUI; // v표시 UI
        public TextMeshProUGUI scoreText; // (선택) 점수 표시 텍스트
    }

    [Header("UI 목록 설정")] [SerializeField] private List<TreeUI> treeUIList;

    public void UpdateTreeStatus(int[] diceValues)
    {
        Dictionary<Trees, int> evaluatedScores = EvaluateAll(diceValues, 0);

        foreach (var uiInfo in treeUIList)
        {
            if (evaluatedScores.TryGetValue(uiInfo.treeType, out int score))
            {
                // Choice는 조건 없이 항상 달성, 그 외 족보는 조건 충족 시(점수 > 0) v표시 활성화
                bool isValid = uiInfo.treeType == Trees.Choice || score > 0;

                if (uiInfo.checkMarkUI != null)
                {
                    uiInfo.checkMarkUI.SetActive(isValid);
                }

                if (uiInfo.scoreText != null)
                {
                    uiInfo.scoreText.text = score.ToString();
                }
            }
        }
    }

    private Dictionary<Trees, int> EvaluateAll(int[] diceValues, int attackPower)
    {
        Dictionary<Trees, int> scores = new Dictionary<Trees, int>();
        int[] counts = new int[7];

        foreach (int val in diceValues)
        {
            if (val >= 1 && val <= 6) counts[val]++;
        }

        int totalSum = diceValues.Sum();

        // 1. 초이스 (전체 눈금 합 / 2 + 공격력)
        scores[Trees.Choice] = (totalSum / 2) + attackPower;

        // --- 여기서부터 동일 눈금 수치 찾기 ---

        // 2. 원 페어 (같은 주사위 2개 합 + 3 + 공격력)
        int onePairFace = 0;
        // 6부터 1까지 거꾸로 찾아서 가장 높은 페어의 눈금을 가져옵니다.
        for (int i = 6; i >= 1; i--)
        {
            if (counts[i] >= 2)
            {
                onePairFace = i;
                break;
            }
        }

        scores[Trees.OnePair] = (onePairFace > 0) ? (onePairFace * 2) + 3 + attackPower : 0;

        // 3. 투 페어 (두 페어 주사위 4개 합 + 4 + 공격력)
        int firstPair = 0, secondPair = 0;
        for (int i = 6; i >= 1; i--)
        {
            if (counts[i] >= 2)
            {
                if (firstPair == 0) firstPair = i;
                else if (secondPair == 0) secondPair = i;
            }
        }

        // 서로 다른 페어가 2개 존재할 경우만 성립
        scores[Trees.TwoPair] = (firstPair > 0 && secondPair > 0)
            ? (firstPair * 2) + (secondPair * 2) + 4 + attackPower
            : 0;

        // 4. 쓰리 오브 어 카인드 (같은 주사위 3개 합 + 6 + 공격력)
        int threeKindFace = 0;
        for (int i = 1; i <= 6; i++)
        {
            if (counts[i] >= 3) threeKindFace = i;
        }

        scores[Trees.Three_Of_AKind] = (threeKindFace > 0) ? (threeKindFace * 3) + 6 + attackPower : 0;

        // 5. 포 오브 어 카인드 (같은 주사위 4개 합 + 12 + 공격력)
        int fourKindFace = 0;
        for (int i = 1; i <= 6; i++)
        {
            if (counts[i] >= 4) fourKindFace = i;
        }

        scores[Trees.Four_Of_AKind] = (fourKindFace > 0) ? (fourKindFace * 4) + 12 + attackPower : 0;

        // 6. 풀 하우스 (20 + 공격력)
        bool has3 = counts.Contains(3);
        bool has2 = counts.Contains(2);
        bool has5 = counts.Contains(5); // 5개가 같아도 풀하우스로 인정
        scores[Trees.FullHouse] = ((has3 && has2) || has5) ? 20 + attackPower : 0;

        // 7 & 8. 스트레이트 판별용 문자열
        string straightStr = string.Join("", diceValues.Distinct().OrderBy(x => x));

        // 7. 스몰 스트레이트 (17 + 공격력)
        bool isSS = straightStr.Contains("1234") || straightStr.Contains("2345") || straightStr.Contains("3456");
        scores[Trees.SmallStraight] = isSS ? 17 + attackPower : 0;

        // 8. 라지 스트레이트 (26 + 공격력)
        bool isLS = straightStr.Contains("12345") || straightStr.Contains("23456");
        scores[Trees.LargeStraight] = isLS ? 26 + attackPower : 0;

        // 9. 야추 (36 + 동일 눈금 * 2 + 공격력)
        int yahtzeeFace = 0;
        for (int i = 1; i <= 6; i++)
        {
            if (counts[i] == 5) yahtzeeFace = i;
        }

        scores[Trees.Yahtzee] = (yahtzeeFace > 0) ? 36 + (yahtzeeFace * 2) + attackPower : 0;

        return scores;
    }

    public void AttackTree()
    {
        Debug.Log("ㅎㅇ");
        GameObject clickBtn = EventSystem.current.currentSelectedGameObject;
        
        TextMeshProUGUI scoreText = clickBtn.transform.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        
        if (scoreText != null)
        {
            // 3. 텍스트 문자열을 int 정수로 변환해서 반환
            if (int.TryParse(scoreText.text, out int score))
            {
                Debug.Log($"선택한 버튼({clickBtn.name})의 점수: {score}");
            }
        }
    }
}