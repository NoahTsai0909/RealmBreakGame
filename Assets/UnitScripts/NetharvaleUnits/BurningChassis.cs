using System.Collections.Generic;
using UnityEngine;

public class BurningChassis : UnitInstance
{
    protected override void UseAbility()
    {
        base.UseAbility();

        UnitInstance target = FindNearestEnemy();
        if (target == null) return;


        CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.ApplyBurn,
                source = this,
                target = target,
                amount = stats.Burn,
                reason = "Burning Chassis Burn"
            }
        );
    }

    public override string GetActiveDescription()
    {
        return ($"[c_burn]Burn[/c] the nearest enemy for [BURN] {stats.Burn}.");
    }

}
