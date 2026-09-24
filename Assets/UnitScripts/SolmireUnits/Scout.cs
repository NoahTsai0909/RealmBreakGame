using System.Collections.Generic;
using UnityEngine;

public class Scout : UnitInstance
{
    private int critBuff;
    List<UnitInstance> targets;

    protected override void UpdateRarityModifiers()
    {
        critBuff = CurrentRarity switch
        {
            Rarity.Uncommon => 7,
            Rarity.Rare => 15,
            Rarity.Epic => 30,
            _ => 7
        };
    }

    protected override void UseAbility()
    {
        base.UseAbility();
        targets = FindSideAllies();

        foreach (UnitInstance target in targets)
        {

            target.TemporaryStatModify(ModifiableStats.CritChance, critBuff);
        }

    }

    public override string GetActiveDescription()
    {
        return ($"[c_side]Side[/c] allies gain [c_crit]{critBuff}[/c] [CRIT].");
    }
}
