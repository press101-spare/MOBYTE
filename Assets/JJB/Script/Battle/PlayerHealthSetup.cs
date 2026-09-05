using UnityEngine;

namespace JJB.Script.Battle
{
    public class PlayerHealthSetup : MonoBehaviour
    {
        private const int MaxHealth = 70;

        private void Awake()
        {
            JJBHealth jjbHealth = GetComponent<JJBHealth>();

            jjbHealth.Initialize(MaxHealth);
        }
    }
}