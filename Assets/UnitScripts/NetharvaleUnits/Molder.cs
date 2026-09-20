using UnityEngine;

public class Molder : UnitInstance
{
    protected override void UseAbility()
    {
        base.UseAbility();
        if (Definition.spawnDefinition != null)
        {
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.Summon,
                source = this,
                spawnPayload = Definition.spawnDefinition,
                reason = "Molder Summon"
            });

            Debug.Log($"Molder routed a summon action to CombatManager at ({row},{col})");
        }
    }

    public override string GetActiveDescription()
    {
        return ($"Summon a thrall.");
    }
}
