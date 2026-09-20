using UnityEngine;
using static CombatEventBus;
using static UnityEngine.GraphicsBuffer;

public class Abjudicator : UnitInstance
{
    private int advanceCount = 1;

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
        if (target.isPlayer != isPlayer) return;
        CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.Advance,
                source = this,
                target = this,
                amount = advanceCount,
                reason = "Abjudicator Passive"
            }
        );
    }

    protected override void UseAbility()
    {
        base.UseAbility();
        UnitInstance enemy = FindRandomEnemy();
        UnitInstance ally = FindRandomAlly();
        if (enemy != null)
        {
            CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.Kill,
                source = this,
                target = enemy,
                reason = "Abjudicator Kill"
            }
        );
        }
        if (ally != null)
        {
            CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.Kill,
                source = this,
                target = ally,
                reason = "Abjudicator Kill",
            }
        );
        }
    }

    public override string GetActiveDescription()
    {
        return ($"Kill a random enemy and a random ally.");
    }

    public override string GetPassiveDescription()
    {
        return ($"When an ally dies, [c_advance]advance[/c] this {advanceCount}.");
    }
}