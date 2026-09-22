using System.Collections.Generic;
using UnityEngine;

public class VenomousGlob : UnitInstance
{
    protected override void UseAbility()
    {
        base.UseAbility();
        UnitInstance target = FindNearestEnemy();
        if (target == null) return;

        CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.ApplyPoison,
                source = this,
                target = target,
                amount = stats.Poison,
                reason = "Venomous Glob Poison",
                isCrit = abilityCrit
            }
        );
    }

    public override string GetActiveDescription()
    {
        return ($"[c_poison]Poison[/c] the nearest enemy for [POISON] {stats.Poison}.");
    }

    public override string GetPassiveDescription()
    {
        return ($"When this dies, [c_poison]Poison[/c] all enemies for [POISON] {stats.Poison}.");
    }

    protected override void OnDeathEffect()
    {
        List<UnitInstance> targets = FindAllEnemies();
        if (targets == null || targets.Count == 0) return;

        foreach (var target in targets)
        {
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.ApplyPoison,
                source = this,
                target = target,
                amount = stats.Poison,
                reason = "Venomous Glob Death Poison",
                isCrit = abilityCrit
            });
        }
    }
}
