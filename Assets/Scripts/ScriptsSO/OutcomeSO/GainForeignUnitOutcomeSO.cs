using UnityEngine;

[CreateAssetMenu(menuName = "Outcomes/Gain Foreign Unit")]
public class GainForeignUnitOutcomeSO : EventOutcomeSO
{
    public UnitTagFlags requiredTags = UnitTagFlags.None;
    public bool bypassExclusivity = false;

    public override void ExecuteOutcome(EventContext context)
    {
        if (context != null && context.generatedUnit != null)
        {
            PlayerUnitManager.Instance.TryAcquireUnit(context.generatedUnit.definition, context.generatedUnit.rarity);
            return;
        }

        Region playerRegion = RunManager.Instance.playerRegion;
        UnitSaveData foreignUnit = UnitGenerationService.GenerateUnit(
            region: null,
            requiredTags: requiredTags,
            bypassExclusive: bypassExclusivity,
            excludedRegion: playerRegion
        );

        if (foreignUnit != null && foreignUnit.definition != null)
        {
            PlayerUnitManager.Instance.TryAcquireUnit(foreignUnit.definition, foreignUnit.rarity);
        }
        else
        {
            Debug.LogWarning("Foreign Unit Generation failed. Ensure UnitDatabase has units outside the player's region.");
        }
    }
}
