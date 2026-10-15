using System;
using UnityEngine;

namespace JJB.Script.Battle.Player.Progression
{
    public class PlayerProfileManager : MonoBehaviour
    {
        public static PlayerProfileManager Instance { get; private set; }

        public PlayerProfile Profile { get; private set; }

        public int currentStageIndex = 1;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            DontDestroyOnLoad(gameObject);

            Profile = new PlayerProfile();
        }

        private void Start()
        {
            AddMoney(300 * currentStageIndex);
        }

        public void AddMoney(int amount)
        {
            if (amount <= 0)
                return;

            Profile.money += amount * 15;
        }
    }
}