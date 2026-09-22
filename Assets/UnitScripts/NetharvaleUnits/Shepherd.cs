using System.Collections.Generic;
using UnityEngine;

public class Shepherd : UnitInstance
{
    private int attackBuff = 10;
    private int advanceCount = 1;
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
            Rarity.Uncommon => 10,
            Rarity.Rare => 20,
            Rarity.Epic => 40,
            _ => 10
        };
    }
    protected override void UseAbility()
    {
        base.UseAbility();
        if (myGrid == null) return;
        int front = -1;
        if (isPlayer)
        {
            front = col + 1;
        }
        else
        {
            front = col - 1;
        }
        if (Definition.spawnDefinition != null)
        {
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.Summon,
                source = this,
                spawnPayload = Definition.spawnDefinition,
                targetPos = new Vector2Int(row, front),
                reason = "Shepherd Summon",
                onFail = () =>
                {
                    UnitInstance frontAlly = myGrid.GetUnitAt(row, front);
                    if (frontAlly != null)
                    {
                        CombatManager.Instance.ExecuteAction(new CombatAction
                        {
                            type = CombatActionType.Advance,
                            source = this,
                            target = frontAlly,
                            amount = advanceCount,
                            reason = "Shepherd Fallback Advance"
                        });
                        CombatManager.Instance.ExecuteAction(new CombatAction
                        {
                            type = CombatActionType.Buff,
                            source = this,
                            target = frontAlly,
                            buffStat = ModifiableStats.Attack,
                            amount = attackBuff,
                            reason = "Shepherd Fallback Buff"
                        });
                    }
                }
            });
        }

    }

    public override string GetActiveDescription()
    {
        return ($"Summon a thrall in front of this. If an ally already occupies the space, instead [c_advance]advance[/c] the ally in front by {advanceCount} and give it [c_attack]+{attackBuff}[/c] [ATK] ");
    }
}
