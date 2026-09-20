using System.Collections.Generic;
using UnityEngine;

public class CombustionGlob : UnitInstance
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
                reason = "Combustion Glob Burn",
                isCrit = abilityCrit
            }
        );
    }

    public override string GetActiveDescription()
    {
        return ($"[c_burn]Burn[/c] the nearest enemy for [BURN] {stats.Burn}.");
    }

    public override string GetPassiveDescription()
    {
        return ($"When this dies, [c_burn]Burn[/c] all enemies for [BURN] {stats.Burn}.");
    }

    protected override void OnDeathEffect()
    {
        List<UnitInstance> targets = FindAllEnemies();
        if (targets == null || targets.Count == 0) return;

        foreach (var target in targets)
        {
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.ApplyBurn,
                source = this,
                target = target,
                amount = stats.Burn,
                reason = "Combustion Glob Death Burn",
                isCrit = abilityCrit
            });
        }
    }
}
