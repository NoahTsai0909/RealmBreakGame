using System.Collections.Generic;
using UnityEngine;

public class Tinderheart : UnitInstance
{
    private int burnBuff = 3;

    protected override void UpdateRarityModifiers()
    {
        burnBuff = CurrentRarity switch
        {
            Rarity.Uncommon => 3,
            Rarity.Rare => 6,
            Rarity.Epic => 12,
            _ => 3
        };
    }
    protected override void UseAbility()
    {
        base.UseAbility();
        List<UnitInstance> targets = FindAllAllies();
        foreach (UnitInstance target in targets)
        {
            target.TemporaryStatModify(ModifiableStats.Burn, burnBuff);
        }
    }

    public override string GetActiveDescription()
    {
        return ($"All allies gain [c_burn]{burnBuff}[/c] [BURN].");
    }
}
