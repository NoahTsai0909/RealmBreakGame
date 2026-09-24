using System.Collections.Generic;
using UnityEngine;

public class Rotbloom : UnitInstance
{
    private int poisonBuff = 1;

    protected override void UpdateRarityModifiers()
    {
        poisonBuff = CurrentRarity switch
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
