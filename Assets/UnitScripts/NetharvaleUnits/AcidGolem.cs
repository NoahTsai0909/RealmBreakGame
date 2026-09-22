using System.Collections.Generic;
using UnityEngine;

public class AcidGolem : UnitInstance
{
    protected override void UseAbility()
    {
        base.UseAbility();
        UnitInstance target = FindRandomEnemy();
        if (target != null)
        {
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.ApplyPoison,
                source = this,
                target = target,
                amount = stats.Poison,
                reason = "AcidGolem Poison",
                isCrit = abilityCrit
            });
        }
        target = FindRandomEnemy();
        if (target != null)
        {
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.ApplyBurn,
                source = this,
                target = target,
                amount = stats.Burn,
                reason = "AcidGolem Burn",
                isCrit = abilityCrit
            });
        }
    }

    public override string GetActiveDescription()
    {
        return ($"[c_poison]Poison[/c] a random enemy for [POISON] {stats.Poison}. [c_burn]Burn[/c] a random enemy for [BURN] {stats.Burn}.");
    }
}