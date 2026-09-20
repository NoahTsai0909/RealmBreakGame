using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VengefulGlob : UnitInstance
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
                reason = "Vengeful Glob Attack",
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
        return ("When this dies, [c_attack]Attack[/c] the nearest enemy for [ATK] {stats.Attack}.");
    }

    protected override void OnDeathEffect()
    {
        UnitInstance target = FindNearestEnemy();
        if (target == null) return;

        CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.Damage,
                source = this,
                target = target,
                amount = stats.Attack,
                reason = "Vengeful Glob Attack",
                isCrit = abilityCrit
            }
        );
    }
}

