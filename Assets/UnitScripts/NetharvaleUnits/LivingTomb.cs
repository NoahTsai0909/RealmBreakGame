using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LivingTomb : UnitInstance
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
                reason = "Living Tomb Attack",
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
        return ("When this dies, summon a Thrall.");
    }

    protected override void OnDeathEffect()
    {
        if (Definition.spawnDefinition != null)
        {
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.Summon,
                source = this,
                spawnPayload = Definition.spawnDefinition,
                // Because it's dying, we forcefully claim its old coordinates!
                targetPos = new Vector2Int(row, col),
                reason = "Living Tomb Death Summon"
            });

            Debug.Log($"Living Tomb routed a summon action to CombatManager at ({row},{col})");
        }
    }
}
