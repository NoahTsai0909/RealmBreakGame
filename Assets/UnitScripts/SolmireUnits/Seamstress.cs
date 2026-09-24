using UnityEngine;
using System.Collections.Generic;

public class Seamstress : UnitInstance
{
    private int maxHealthBuffPercent = 25;
    private UnitInstance frontAlly;
    private bool buffApplied = false;
    private int buffAmount = 0;
    private int originalMaxHP = 0;


    protected override void UpdateRarityModifiers()
    {
        maxHealthBuffPercent = CurrentRarity switch
        {
            Rarity.Rare => 25,
            Rarity.Epic => 50,
            _ => 25
        };
    }

    public override void CombatStartEffect()
    {
        List<UnitInstance> adjacentAllies = FindAdjacentAllies();
        int expectedCol = isPlayer ? col + 1 : col - 1;

        foreach (UnitInstance adjacentAlly in adjacentAllies)
        {

            if (adjacentAlly.col == expectedCol)
            {
                frontAlly = adjacentAlly;
                ApplyBuff();
                break;
            }
               
        }
    }

    private void ApplyBuff()
    {
        if (frontAlly == null || buffApplied) return;
        originalMaxHP = frontAlly.GetCurrentHP();
        buffAmount = (originalMaxHP * maxHealthBuffPercent) / 100;
        CombatManager.Instance.ExecuteAction(new CombatAction
        {
            type = CombatActionType.Buff,
            source = this,
            target = frontAlly,
            buffStat = ModifiableStats.MaxHP,
            amount = buffAmount
        });

        Debug.Log($"Applied +{maxHealthBuffPercent}% max HP to {frontAlly.unitName}. +{buffAmount} HP");
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
                reason = "Seamstress Attack",
                isCrit = abilityCrit
            }
        );
    }
    public override string GetActiveDescription()
    {
        return ($"[c_attack]Attack[/c] the nearest enemy for [ATK] {stats.Attack}.");
    }

    public override string GetPassiveDescription()
    {
        return ($"Combat Start: The ally in front of this has [c_maxhealth]+{maxHealthBuffPercent}%[/c] [MAXHEALTH].");
    }
}
