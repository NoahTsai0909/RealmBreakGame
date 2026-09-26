using UnityEngine;

public class UnstableCrown : UnitInstance, IConsumable
{

    public bool OnConsume(UnitInstance target)
    {
        if (target == null) return false;

        if (target.CurrentRarity == Rarity.Common)
        {
            target.UpgradeTier();
            return true;
        }
        return false;
    }

    public override string GetActiveDescription()
    {
        return "[c_consume]Consume[/c] this to upgrade the tier of a [c_common]common[/c] unit.";
    }
}

