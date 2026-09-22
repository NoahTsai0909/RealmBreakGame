using UnityEngine;
using static CombatEventBus;

public class Echo : UnitInstance
{

    protected override void UseAbility()
    {
        base.UseAbility();
        UnitInstance ally = FindRandomAlly();
        UnitDefinition allyDef = null;
        if (ally != null)
        {
            allyDef = ally.Definition;
            CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.Kill,
                source = this,
                target = ally,
                reason = "Echo Kill",
            }
        );
        }
        if (allyDef != null)
        {
            CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.Summon,
                source = this,
                spawnPayload = allyDef,
                reason = "Echo Summon",
            }
        );
        }
    }

    public override string GetActiveDescription()
    {
        return ($"Kill a random ally. Summon a copy of it.");
    }

}