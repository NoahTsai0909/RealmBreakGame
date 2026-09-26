using UnityEngine;

public class RefreshingMead : UnitInstance, IConsumable
{
    private int healModifier = 4;

    public override void InitializeFromSaveData(UnitSaveData data)
    {
        base.InitializeFromSaveData(data);
        if (CurrentRarity == Rarity.Common)
        {
            healModifier = 4;
        }
        if (CurrentRarity == Rarity.Uncommon)
        {
            healModifier = 8;
        }
        else if (CurrentRarity == Rarity.Rare)
        {
            healModifier = 16;
        }
        else if (CurrentRarity == Rarity.Epic)
        {
            healModifier = 32;
        }
        else
        {
            healModifier = 4;
        }
    }

    public bool OnConsume(UnitInstance target)
    {
        if (target == null) return false;
        RunManager.Instance.GetPermanentStatsForUnit(target.id).bonusHeal += healModifier;
        target.RecalculateStats();
        return true;
    }

    public override string GetActiveDescription()
    {
        return ($"[c_consume]Consume[/c] this to grant [c_heal]{healModifier}[/c] [HEAL] permanently.");
    }
}
