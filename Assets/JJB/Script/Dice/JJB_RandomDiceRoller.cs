using UnityEngine;

namespace JJB.Script
{
    public class JJB_RandomDiceRoller : MonoBehaviour, IDiceRoller
    {
        public int Roll()
        {
            return Random.Range(1, 7);
        }
    }
}