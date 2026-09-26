using UnityEngine;

public class OverclockModule : UnitInstance, IConsumable
{
    public int buffValue = 3;

    protected override void UpdateRarityModifiers()
    {
        buffValue = CurrentRarity switch
        {
            Rarity.Uncommon => 3,
            Rarity.Rare => 6,
            Rarity.Epic => 9,
            _ => 3
        };
    }


    public bool OnConsume(UnitInstance target)
    {
        if (target == null) return false;
        RunManager.Instance.GetPermanentStatsForUnit(target.id).cooldownReduction += buffValue;
        target.RecalculateStats();
        return true;
    }

    public override string GetActiveDescription()
    {
        return ($"[c_consume]Consume[/c] this to reduce target cooldown by {buffValue}% permanently.");
    }
}
