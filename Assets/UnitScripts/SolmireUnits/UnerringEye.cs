using System.Collections.Generic;
using UnityEngine;

public class UnerringEye : UnitInstance
{
    private int hasteModifier = 0;
    private int critModifier = 25;

    protected override void UpdateRarityModifiers()
    {
        hasteModifier = CurrentRarity switch
        {
            Rarity.Rare => 0,
            Rarity.Epic => 1,
            _ => 0
        };
        critModifier = CurrentRarity switch
        {
            Rarity.Rare => 25,
            Rarity.Epic => 50,
            _ => 25
        };
    }

    protected override int GetRarityAdjustedHaste()
    {
        return hasteModifier;
    }


    protected override void UseAbility()
    {
        base.UseAbility();
        List<UnitInstance> targets = FindAdjacentAllies();
        foreach (UnitInstance target in targets)
        {
            CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.ApplyHaste,
                source = this,
                target = target,
                amount = stats.Haste,
                reason = "Unerring Eye Haste"
            }

            );
        }
    }
    public override void RemoveAuras()
    {
        foreach (UnitInstance target in auraTargets)
        {
            if (target != null)
            {
                target.TemporaryStatModify(ModifiableStats.CritChance, -critModifier);
            }
        }
        base.RemoveAuras(); // Clears the list
    }

    public override void ApplyAuras()
    {

        if (myGrid == null) return;

        auraTargets = FindAdjacentAllies();

        if (auraTargets == null) return;

        foreach (UnitInstance target in auraTargets)
        {
            if (target != null)
            {
                target.TemporaryStatModify(ModifiableStats.CritChance, critModifier);
            }
        }
    }

    public override string GetActiveDescription()
    {
        return ($"[c_haste]Haste[/c] [c_adjacent]adjacent[/c] allies for [HASTE] {stats.Haste}.");
    }

    public override string GetPassiveDescription()
    {
        return ($"[c_adjacent]Adjacent[/c] allies have [c_crit]+{critModifier}[/c] [CRIT].");
    }
}
