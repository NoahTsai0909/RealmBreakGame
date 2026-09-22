using System.Collections.Generic;
using UnityEngine;

public class Rotbloom : UnitInstance
{
    private int poisonBuff = 1;

    public override void InitializeFromSaveData(UnitSaveData data)
    {
        base.InitializeFromSaveData(data);
        poisonBuff = findBuff(CurrentRarity);
    }


    public override void InitializeEnemy(UnitDefinition def, Rarity rarity)
    {
        base.InitializeEnemy(def, rarity);
        poisonBuff = findBuff(rarity);
    }

    private int findBuff(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => 1,
            Rarity.Uncommon => 2,
            Rarity.Rare => 4,
            Rarity.Epic => 8,
            _ => 2
        };
    }
    protected override void UseAbility()
    {
        base.UseAbility();
        List<UnitInstance> targets = FindAllAllies();
        foreach (UnitInstance target in targets)
        {
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.Buff,
                source = this,
                target = target,
                amount = poisonBuff,
                buffStat = ModifiableStats.Poison,
                reason = "Rotbloom Buff"
            });
        }
    }

    public override string GetActiveDescription()
    {
        return ($"All allies gain [c_poison]{poisonBuff}[/c] [POISON].");
    }
}
