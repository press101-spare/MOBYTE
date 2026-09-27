using UnityEngine;

namespace JJB.Script.Battle.Player.Progression
{
    public class PlayerProfileManager : MonoBehaviour
    {
        public static PlayerProfileManager Instance { get; private set; }

        public PlayerProfile Profile { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetInstance()
        {
            Instance = null;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnsureInstance()
        {
            if (Instance != null)
                return;

            // If the starting scene already contains a manager, its Awake will
            // initialize it. Otherwise create one so every scene can run alone.
            if (FindFirstObjectByType<PlayerProfileManager>() != null)
                return;

            GameObject managerObject =
                new GameObject(nameof(PlayerProfileManager));

            managerObject.AddComponent<PlayerProfileManager>();
        }

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

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
