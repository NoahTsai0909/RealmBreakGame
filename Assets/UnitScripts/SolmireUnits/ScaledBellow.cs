using UnityEngine;

public class ScaledBellow : UnitInstance
{
    private int attackModifier = 10;
    private int burnModifier = 2;
    private int mutationTriggerThreshold = 5;
    private int mutationTriggerCount = 0;

    protected override void UpdateRarityModifiers()
    {
        attackModifier = CurrentRarity switch
        {
            Rarity.Uncommon => 10,
            Rarity.Rare => 20,
            Rarity.Epic => 30,
            _ => 30
        };
        burnModifier = CurrentRarity switch
        {
            Rarity.Uncommon => 2,
            Rarity.Rare => 4,
            Rarity.Epic => 6,
            _ => 2
        };
    }

    public override void EnterCombat(GridManager grid, int row, int col, bool isPlayer, bool startCombat = true)
    {
        base.EnterCombat(grid, row, col, isPlayer, startCombat);
        CombatEventBus.OnActionResolved += HandleCombatAction;
    }

    private void OnDestroy()
    {
        CombatEventBus.OnActionResolved -= HandleCombatAction;
    }

    protected override void HandleCombatAction(CombatAction action)
    {
        if (action.source == null) return;
        if (action.source.isPlayer != this.isPlayer) return;
        if (action.isAoEExtraHit) return;
        if (action.type == CombatActionType.ApplyBurn)
        {
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.Buff,
                source = this,
                target = action.source,
                buffStat = ModifiableStats.Burn,
                amount = burnModifier
            });
            mutationTriggerCount++;
        }
        if (action.type == CombatActionType.Damage){
            CombatManager.Instance.ExecuteAction(new CombatAction
            {
                type = CombatActionType.Buff,
                source = this,
                target = action.source,
                buffStat = ModifiableStats.Attack,
                amount = attackModifier
            });
            mutationTriggerCount++;
        }
        if (currentSuffix != null)
        {
            if (mutationTriggerCount >= mutationTriggerThreshold)
            {
                mutationTriggerCount = 0;
                currentSuffix.ExecuteEffect(this);
            }
        }
    }

    public override string GetPassiveDescription()
    {
        return ($"When an ally [c_attack]attacks[/c], give it [ATK] {attackModifier}. \n When an ally [c_burn]burns[/c], give it [BURN] {burnModifier}.");
    }

    public override string GetMutationTriggerText()
    {
        return ($"<br>Every {mutationTriggerThreshold} times an ally [c_attack]attacks[/c] or [c_burn]burns[/c], ");
    }
}

