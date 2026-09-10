namespace JJB.Script.Battle.Enemy
{
    public interface IEnemyTurnStartAbility
    {
        void OnTurnStart();
    }

    public interface IEnemyTurnEndAbility
    {
        void OnTurnEnd();
    }

    public interface IEnemyAttackModifier
    {
        int ModifyAttackDamage(int damage);
    }

    public interface IPlayerAttackModifier
    {
        int ModifyPlayerDamage(int damage);
    }

    public interface IRerollModifier
    {
        int ModifyRerollCost(int cost);
    }
    
    public interface IMoneyStealAbility
    {
        void TrySteal();
    }
}