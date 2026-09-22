using UnityEngine;

public class Victim : UnitInstance
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
                reason = "Victim Death Summon"
            });
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.Summon,
                source = this,
                spawnPayload = Definition.spawnDefinition,
                reason = "Victim Death Summon"
            });
        }
        CombatManager.Instance.ExecuteAction(new CombatAction
        {
            type = CombatActionType.Kill,
            source = this,
            target = this,
            reason = "Victim Self Kill"
        });

    }

    public override string GetActiveDescription()
    {
        return ($"Summon 2 thralls. Kill this.");
    }
}
