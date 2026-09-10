using JJB.Script.Battle.Enemy;
using JJB.Script.Battle.Player;
using UnityEngine;

namespace JJB.Script.Battle
{
    public class BattleInstaller : MonoBehaviour
    {
        [SerializeField] private PlayerBattleActor player;
        [SerializeField] private EnemyBattleActor enemy;

        private BattleSystem _battleSystem;

        private void Start()
        {
            _battleSystem = GetComponent<BattleSystem>();
            _battleSystem.Initialize(player, enemy);
        }
    }
}