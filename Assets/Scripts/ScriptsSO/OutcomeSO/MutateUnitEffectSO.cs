using UnityEngine;

[CreateAssetMenu(menuName = "Unit Effects/Mutate Unit")]
public class MutateUnitEffectSO : UnitTargetEffectSO
{
    public override void ApplyEffect(RunManager.UnitPlacement placement)
    {
        // 1. Safety check
        if (placement == null || placement.unitData == null) return;

        Debug.Log($"Mutating unit: {placement.unitData.definition.unitName}");

        MutationPrefixSO rolledPrefix = null;
        MutationSuffixSO rolledSuffix = null;

        // 2. Grab the master list of prefixes from RunManager
        var allPrefixes = RunManager.Instance.allAvailablePrefixes;
        if (allPrefixes != null && allPrefixes.Count > 0)
        {
            // Pick a random prefix
            rolledPrefix = allPrefixes[Random.Range(0, allPrefixes.Count)];

            // Pick a random suffix from that specific prefix's allowed buckets
            if (rolledPrefix.allowedSuffixes != null && rolledPrefix.allowedSuffixes.Count > 0)
            {
                rolledSuffix = rolledPrefix.allowedSuffixes[Random.Range(0, rolledPrefix.allowedSuffixes.Count)];
            }
        }

        // 3. Apply the mutations directly to the unit!
        placement.unitData.prefix = rolledPrefix;
        placement.unitData.suffix = rolledSuffix;
    }
}
