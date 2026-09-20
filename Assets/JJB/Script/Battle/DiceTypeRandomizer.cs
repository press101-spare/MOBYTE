using System.Collections.Generic;
using UnityEngine;

namespace JJB.Script.Battle
{
    public static class DiceTypeRandomizer
    {
        public static List<DiceSO_JCY> Randomize(List<DiceSO_JCY> originalDice)
        {
            List<DiceSO_JCY> result = new List<DiceSO_JCY>();

            DiceManager_JCY manager = DiceManager_JCY.Instance;

            if (manager == null || manager.allDiceSo == null || manager.allDiceSo.Length == 0)
                return new List<DiceSO_JCY>(originalDice);

            for (int i = 0; i < originalDice.Count; i++)
            {
                DiceSO_JCY randomSO = manager.allDiceSo[Random.Range(0, manager.allDiceSo.Length)];

                result.Add(randomSO);
            }

            return result;
        }
    }
}