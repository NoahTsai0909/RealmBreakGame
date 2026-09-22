using UnityEngine;

public class WidowsKiss : UnitInstance, IConsumable
{
    private int poisonModifier = 1;

    public override void InitializeFromSaveData(UnitSaveData data)
    {
        base.InitializeFromSaveData(data);
        if (CurrentRarity == Rarity.Common)
        {
            poisonModifier = 1;
        }
        if (CurrentRarity == Rarity.Uncommon)
        {
            poisonModifier = 2;
        }
        else if (CurrentRarity == Rarity.Rare)
        {
            poisonModifier = 4;
        }
        else if (CurrentRarity == Rarity.Epic)
        {
            poisonModifier = 8;
        }
        else
        {
            poisonModifier = 1;
        }
    }

    public bool OnConsume(UnitInstance target)
    {
        if (target == null) return false;
        RunManager.Instance.GetPermanentStatsForUnit(target.id).bonusPoison += poisonModifier;
        target.RecalculateStats();
        return true;
    }

    public override string GetActiveDescription()
    {
        return ($"Consume this to grant [c_poison]{poisonModifier}[/c] [POISON] permanently.");
    }
}
