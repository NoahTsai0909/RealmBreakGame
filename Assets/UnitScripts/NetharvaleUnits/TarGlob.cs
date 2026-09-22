using System.Collections.Generic;
using UnityEngine;

public class TarGlob : UnitInstance
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
                reason = "Tar Glob Slow"
            }
        );
    }

    public override string GetActiveDescription()
    {
        return ($"[c_slow]Slow[/c] the nearest enemy for [SLOW] {stats.Slow}.");
    }

    public override string GetPassiveDescription()
    {
        return ($"When this dies, [c_slow]Slow[/c] all enemies for [SLOW] {stats.Slow}.");
    }

    protected override void OnDeathEffect()
    {
        List<UnitInstance> targets = FindAllEnemies();
        if (targets == null || targets.Count == 0) return;

        foreach (var target in targets)
        {
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.ApplySlow,
                source = this,
                target = target,
                amount = stats.Slow,
                reason = "Tar Glob Death Slow"
            });
        }
    }


}
