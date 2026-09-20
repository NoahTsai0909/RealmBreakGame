using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Caustic : UnitInstance
{
    public override void EnterCombat(GridManager grid, int row, int col, bool isPlayer, bool startCombat = true)
    {
        base.EnterCombat(grid, row, col, isPlayer, startCombat);

        CombatEventBus.OnActionResolved += HandleActionResolved;

    }

    private void OnDestroy()
    {
        CombatEventBus.OnActionResolved -= HandleActionResolved;
    }

    private void HandleActionResolved(CombatAction action)
    {
        if (action.source == null) return;
        if (action.type != CombatActionType.Kill) return;
        if (action.source != this) return;
        CombatManager.Instance.ExecuteAction(
                new CombatAction
                {
                    type = CombatActionType.Buff,
                    source = this,
                    target = this,
                    amount = stats.Poison,
                    buffStat = ModifiableStats.Poison,
                    reason = "Caustic Poison Buff"
                }
        );
    }


    protected override void UseAbility()
    {
        base.UseAbility();
        UnitInstance target = FindNearestEnemy();

        if (target != null)
        {
            CombatManager.Instance.ExecuteAction(
                new CombatAction
                {
                    type = CombatActionType.ApplyPoison,
                    source = this,
                    target = target,
                    amount = stats.Poison,
                    reason = "Caustic Poison",
                    isCrit = abilityCrit
                }
            );

        }
        List<UnitInstance> targets = FindAllAllies();
        foreach (UnitInstance ally in targets)
        {
            bool sameRow = ally.row == row;
            bool behind;
            if (isPlayer)
            {
                behind = ally.col == col - 1;
            }
            else
            {
                behind = ally.col == col + 1;
            }
            if (sameRow && behind)
            {
                CombatManager.Instance.ExecuteAction(
                new CombatAction
                {
                    type = CombatActionType.Kill,
                    source = this,
                    target = ally,
                    reason = "Caustic Kill"
                }

                );
            }

        }
    }

    public override string GetActiveDescription()
    {
        return ($"Kill the ally behind this. [c_poison]Poison[/c] the nearest enemy for [POISON] {stats.Poison}.");
    }

    public override string GetPassiveDescription()
    {
        return ($"When this kills an ally, double the [POISON] on this.");
    }


}
