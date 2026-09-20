using System.Collections.Generic;
using UnityEngine;

public class Mother : UnitInstance
{
    private int attackBuff = 15;

    public override void InitializeFromSaveData(UnitSaveData data)
    {

        base.InitializeFromSaveData(data);
        attackBuff = findBuff(CurrentRarity);
    }

    public override void InitializeEnemy(UnitDefinition def, Rarity rarity)
    {
        base.InitializeEnemy(def, rarity);
        attackBuff = findBuff(rarity);
    }

    private int findBuff(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Rare => 15,
            Rarity.Epic => 30,
            _ => 15
        };
    }

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
                reason = "Mother Summon",
                onFail = () =>
                {
                    List<UnitInstance> allies = FindAllAllies();
                    foreach (UnitInstance ally in allies)
                    {
                        CombatManager.Instance.ExecuteAction(new CombatAction
                        {
                            type = CombatActionType.Buff,
                            source = this,
                            target = ally,
                            buffStat = ModifiableStats.Attack,
                            amount = attackBuff,
                            reason = "Summon Failed Fallback Buff"
                        });
                    }
                }
            });
        }
    }

    public override string GetActiveDescription()
    {
        return ($"Summon a thrall. If there is no available space, give all allies [c_attack]+{attackBuff}[/c] [ATK].");
    }
}

