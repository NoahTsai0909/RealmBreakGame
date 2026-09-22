using System.Collections.Generic;
using UnityEngine;

public class Paralyzer : UnitInstance
{
    protected override void UseAbility()
    {
        base.UseAbility();

        UnitInstance target = FindNearestEnemy();
        if (target == null) return;


        CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.ApplySlow,
                source = this,
                target = target,
                amount = stats.Slow,
                reason = "Paralyzer Slow"
            }
        );
        CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.ApplyPoison,
                source = this,
                target = target,
                amount = stats.Poison,
                reason = "Paralyzer Poison"
            }
        );

    }

    public override string GetActiveDescription()
    {
        return ($"[c_slow]Slow[/c] the nearest enemy for [SLOW] {stats.Slow}. [c_poison]Poison[/c] the nearest enemy for [POISON] {stats.Poison}.");
    }
}
