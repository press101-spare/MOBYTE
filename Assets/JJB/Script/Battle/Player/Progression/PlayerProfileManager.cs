using UnityEngine;

namespace JJB.Script.Battle.Player.Progression
{
    public class PlayerProfileManager : MonoBehaviour
    {
        public static PlayerProfileManager Instance { get; private set; }

        public PlayerProfile Profile { get; private set; }

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
    }
}