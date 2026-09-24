using UnityEngine;

public class Sharpsight : UnitInstance, IConsumable
{
    public int buffValue = 4;

    protected override void UpdateRarityModifiers()
    {
        buffValue = CurrentRarity switch
        {
            Rarity.Common => 4,
            Rarity.Uncommon => 8,
            Rarity.Rare => 16,
            Rarity.Epic => 32,
            _ => 4
        };
    }

    public bool OnConsume(UnitInstance target)
    {
        if (target == null) return false;
        RunManager.Instance.GetPermanentStatsForUnit(target.id).bonusCritChance += buffValue;
        target.RecalculateStats();
        return true;
    }

    public override string GetActiveDescription()
    {
        return ($"Consume this to grant [c_crit]{buffValue}[/c] [CRIT] permanently.");
    }
}
