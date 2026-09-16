using System;
using System.Collections.Generic;

namespace JJB.Script.Battle.Player.Progression
{
    [Serializable]
    public class PlayerProfile
    {
        public int level = 1;
        public int currentExp;
        public int money = 10000;

        public PlayerStats stats = new();

        public List<string> unlockedDice = new();
        public List<string> unlockedTitles = new();

        public List<string> discoveredDice = new();
        public List<string> discoveredEnemies = new();

        public PlayerSettings settings = new();
        public TutorialProgress tutorial = new();
    }
}