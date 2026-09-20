using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class WrathfulGlob : UnitInstance
{
    protected override void UseAbility()
    {
        base.UseAbility();
        UnitInstance target = FindNearestEnemy();
        if (target == null) return;

        CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.Damage,
                source = this,
                target = target,
                amount = stats.Attack,
                reason = "Wrathful Glob Attack",
                isCrit = abilityCrit
            }
        );
    }

    public override string GetActiveDescription()
    {
        return ($"[c_attack]Attack[/c] the nearest enemy for [ATK] {stats.Attack}.");
    }

    public override string GetPassiveDescription()
    {
        return ($"When this dies, all allies gain [c_attack]+ {stats.Attack}[/c] [ATK].");
    }

    protected override void OnDeathEffect()
    {
        List<UnitInstance> targets = FindAllAllies();
        if (targets == null || targets.Count == 0) return;

        foreach (var target in targets)
        {
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.Buff,
                source = this,
                target = target,
                buffStat = ModifiableStats.Attack,
                amount = stats.Attack
            });
        }
    }
}

