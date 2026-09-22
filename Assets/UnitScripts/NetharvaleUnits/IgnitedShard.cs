using System.Collections.Generic;
using UnityEngine;
using static CombatEventBus;

public class IgnitedShard : UnitInstance
{
    private int burnBuff = 1;
    private int mutationTriggerCount = 0;
    private int mutationTriggerThreshold = 3;

    public override void InitializeFromSaveData(UnitSaveData data)
    {

        base.InitializeFromSaveData(data);
        burnBuff = findBuff(CurrentRarity);
    }

    public override void InitializeEnemy(UnitDefinition def, Rarity rarity)
    {
        base.InitializeEnemy(def, rarity);
        burnBuff = findBuff(rarity);
    }

    private int findBuff(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => 1,
            Rarity.Uncommon => 2,
            Rarity.Rare => 3,
            Rarity.Epic => 4,
            _ => 1
        };
    }

    public override string GetMutationTriggerText()
    {
        return ($"<br>Every {mutationTriggerThreshold} times an [c_adjacent]adjacent[/c] ally uses an ability, ");
    }
    public override string GetPassiveDescription()
    {
        return ($"When this unit survives combat, all allies gain [c_burn]+{burnBuff}[/c] [BURN] permanently.");
    }

    protected override void OnTierUpgraded()
    {
        base.OnTierUpgraded();
        burnBuff = findBuff(CurrentRarity);
    }

    public override void EnterCombat(GridManager grid, int row, int col, bool isPlayer, bool startCombat = true)
    {
        base.EnterCombat(grid, row, col, isPlayer, startCombat);

        CombatEventBus.OnCombatEvent += HandleCombatEvent;
        CombatEventBus.OnCombatEnd += HandleCombatEnd;
    }

    private void OnDestroy()
    {
        CombatEventBus.OnCombatEvent -= HandleCombatEvent;
        CombatEventBus.OnCombatEnd -= HandleCombatEnd;
    }

    protected override void HandleCombatEvent(CombatEventType type, UnitInstance source, UnitInstance target, int amount)
    {
        if (!inCombat || currentSuffix == null) return;
        if (type != CombatEventType.AbilityUsed) return;
        if (source.isPlayer != this.isPlayer) return;
        if (auraTargets?.Contains(source) == false) return;
        mutationTriggerCount++;
        if (mutationTriggerCount == mutationTriggerThreshold)
        {
            mutationTriggerCount = 0;
            currentSuffix.ExecuteEffect(this);
        }
    }

    private void HandleCombatEnd()
    {
        List<UnitInstance> allies = FindAllAllies();
        for (int i = 0; i < allies.Count; i++)
        {
            if (allies[i] != null)
            {
                RunManager.Instance.GetPermanentStatsForUnit(allies[i].id).bonusBurn += burnBuff;
            }
        }
    }

}
