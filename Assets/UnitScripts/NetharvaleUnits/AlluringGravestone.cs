using UnityEngine;
using static CombatEventBus;

public class AlluringGravestone : UnitInstance
{
    int slowCount = 2;

    public override string GetPassiveDescription()
    {
        return ($"When an ally dies, [c_attack]attack[/c] the nearest enemy for [ATK] {stats.Attack} damage and [c_slow]slow[/c] {slowCount} random enemies for {stats.Slow}.");
    }

    public override void EnterCombat(GridManager grid, int row, int col, bool isPlayer, bool startCombat = true)
    {
        base.EnterCombat(grid, row, col, isPlayer, startCombat);

        CombatEventBus.OnCombatEvent += HandleCombatEvent;
    }

    private void OnDestroy()
    {
        CombatEventBus.OnCombatEvent -= HandleCombatEvent;
    }

    protected override void HandleCombatEvent(CombatEventType type, UnitInstance source, UnitInstance target, int amount)
    {
        if (type != CombatEventType.UnitDied) return;
        if (source.isPlayer != isPlayer) return;
        UnitInstance enemy = FindNearestEnemy();
        if (enemy == null) return;

        CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.Damage,
                source = this,
                target = enemy,
                amount = stats.Attack,
                reason = "Alluring Gravestone Attack"
            }
        );

        for (int i = 0; i < slowCount; i++)
        {
            UnitInstance randomEnemy = FindRandomEnemy();
            if (randomEnemy == null) break;
            CombatManager.Instance.ExecuteAction(
                new CombatAction
                {
                    type = CombatActionType.ApplySlow,
                    source = this,
                    target = randomEnemy,
                    amount = stats.Slow,
                    reason = "Alluring Gravestone Slow"
                }
            );
        }

        if (currentSuffix != null)
        {
            currentSuffix.ExecuteEffect(this);
        }
    }

}
