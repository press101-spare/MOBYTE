using System;
using System.Collections.Generic;

namespace JJB.Script.Battle.Player.Progression
{
    [Serializable]
    public class PlayerProfile
    {
        public int level = 1;
        public int currentExp;
        public int money = 0;

        public PlayerStats stats = new();
    }
}