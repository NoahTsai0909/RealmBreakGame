using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ClumsyKnight : UnitInstance
{
    private int shieldBuff;

    protected override void UpdateRarityModifiers()
    {
        shieldBuff = CurrentRarity switch
        {
            Rarity.Common => 5,
            Rarity.Uncommon => 10,
            Rarity.Rare => 20,
            Rarity.Epic => 40,
            _ => 5
        };
    }

    protected override void UseAbility()
    {
        base.UseAbility();
        UnitInstance target = FindNearestEnemy();
        if (target == null) return;

        CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.Damage,
                source = this,
                target = target,
                amount = stats.Attack,
                reason = "Clumsy Knight Attack",
                isCrit = abilityCrit
            }
        );

    }

    public override void CombatStartEffect()
    {
        List<UnitInstance> allies = FindAdjacentAllies();
        CombatManager.Instance.ExecuteAction(
            new CombatAction
            {
                type = CombatActionType.Shield,
                source = this,
                target = this,
                amount = shieldBuff * allies.Count,
                reason = "Clumsy Knight Shield"
            }
        );
    }

    public override string GetActiveDescription()
    {
        return ($"[c_attack]Attack[/c] the nearest enemy for [ATK] {stats.Attack}.");
    }

    public override string GetPassiveDescription()
    {
        return ($"Combat Start: [c_shield]shield[/c] this for [SHIELD] {shieldBuff} for each [c_adjacent]adjacent[/c] ally.");
    }

}
