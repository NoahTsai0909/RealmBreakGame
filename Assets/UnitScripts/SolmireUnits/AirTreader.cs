using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AirTreader : UnitInstance
{
    private int hasteModifier = 0;

    protected override void UpdateRarityModifiers()
    {
        hasteModifier = CurrentRarity switch
        {
            Rarity.Rare => 0,
            Rarity.Epic => 1,
            _ => 0
        };
    }

    protected override void UseAbility()
    {
        base.UseAbility();
        if (currentEnergy <= 0) return;
        List<UnitInstance> targets = FindAllAllies();
        foreach (UnitInstance target in targets)
        {
            CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.ApplyHaste,
                source = this,
                target = target,
                amount = stats.Haste,
                reason = "AirTreader Haste"
            }

            );
        }
    }

    protected override int GetRarityAdjustedHaste()
    {
        return hasteModifier;
    }

    public override string GetActiveDescription()
    {
        return ($"[c_haste]Haste[/c] all allies for [HASTE] {stats.Haste}.");
    }
}
