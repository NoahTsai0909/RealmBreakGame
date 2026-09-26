using UnityEngine;
using static CombatEventBus;

public class Havoc : UnitInstance
{

    public override void EnterCombat(GridManager grid, int row, int col, bool isPlayer, bool startCombat = true)
    {
        base.EnterCombat(grid, row, col, isPlayer, startCombat);

        CombatEventBus.OnCombatEvent += HandleCombatEvent;
    }

    private void OnDestroy()
    {
        CombatEventBus.OnCombatEvent -= HandleCombatEvent;
    }

    protected override void HandleCombatEvent(CombatEventType type, UnitInstance source, UnitInstance target, int amount)
    {
        if (type != CombatEventType.UnitDied) return;
        if (source != this) return;
        if (target == this) return;
        CombatManager.Instance.ExecuteAction(
                new CombatAction
                {
                    type = CombatActionType.Buff,
                    source = this,
                    target = this,
                    buffStat = ModifiableStats.Multicast,
                    amount = 1,
                    reason = "Havoc Multicast Buff"
                }
        );
    }

    protected override void UseAbility()
    {
        base.UseAbility();
        UnitInstance randomEnemy = FindRandomEnemy();
        if (randomEnemy != null)
        {
            CombatManager.Instance.ExecuteAction(
                new CombatAction
                {
                    type = CombatActionType.Damage,
                    source = this,
                    target = randomEnemy,
                    amount = stats.Attack,
                    reason = "Havoc Enemy Damage",
                    isCrit = abilityCrit
                }
            );
        }
        UnitInstance randomAlly = FindRandomAlly(excludeSelf: true);
        if (randomAlly != null)
        {
            CombatManager.Instance.ExecuteAction(
                new CombatAction
                {
                    type = CombatActionType.Damage,
                    source = this,
                    target = randomAlly,
                    amount = stats.Attack,
                    reason = "Havoc Friendly Fire",
                    isCrit = abilityCrit
                }
            );
        }
    }

    public override string GetActiveDescription()
    {
        return ($"[c_attack]Attack[/c] a random enemy and a random ally for [ATK] {stats.Attack}.");
    }

    public override string GetPassiveDescription()
    {
        return ($"When this kills a unit, gain [c_multicast]+1[/c] [MULTICAST].");
    }
}