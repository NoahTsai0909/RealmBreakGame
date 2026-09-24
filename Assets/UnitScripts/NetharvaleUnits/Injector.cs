using UnityEngine;

public class Injector : UnitInstance
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
            _ => 1
        };
    }

    public override void EnterCombat(GridManager grid, int row, int col, bool isPlayer, bool startCombat = true)
    {
        base.EnterCombat(grid, row, col, isPlayer, startCombat);

        CombatEventBus.OnCombatEnd += HandleCombatEnd;
    }

    protected override void UseAbility()
    {
        base.UseAbility();
        UnitInstance target = FindNearestEnemy();

        if (target != null)
        {
            CombatManager.Instance.ExecuteAction(
                new CombatAction
                {
                    type = CombatActionType.ApplyPoison,
                    source = this,
                    target = target,
                    amount = stats.Poison,
                    reason = "Injector Poison",
                    isCrit = abilityCrit
                }
            );

        }
    }

    public override string GetActiveDescription()
    {
        return ($"[c_poison]Poison[/c] the nearest enemy for [POISON] {stats.Poison}.");
    }

    public override string GetPassiveDescription()
    {
        return ($"When this unit survives combat, gain [c_poison]+{poisonBuff}[/c] [POISON] permanently.");
    }

    private void OnDestroy()
    {
        CombatEventBus.OnCombatEnd -= HandleCombatEnd;
    }

    private void HandleCombatEnd()
    {
        RunManager.Instance.GetPermanentStatsForUnit(id).bonusPoison += poisonBuff;
    }
}
