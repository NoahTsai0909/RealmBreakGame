using System.Collections.Generic;
using UnityEngine;

public class MetaManager : MonoBehaviour
{
    public static MetaManager Instance { get; private set; }

    public MetaSaveData metaData;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load the data as soon as the game opens
        metaData = SaveManager.Load<MetaSaveData>("meta_save.json");
    }

    // Call this when the player encounters a new unit!
    public void UnlockUnitInCompendium(UnitDefinition def)
    {
        if (def == null) return;

        string unitName = def.name;

        if (!metaData.unlockedCompendiumUnits.Contains(unitName))
        {
            metaData.unlockedCompendiumUnits.Add(unitName);
            Debug.Log($"[Compendium] New unit unlocked: {unitName}!");

            SaveManager.Save("meta_save.json", metaData);
        }
    }

    public void RegisterWinningTeam(List<RunManager.UnitPlacement> finalTeam)
    {
        bool madeChanges = false;

        foreach (var placement in finalTeam)
        {
            if (placement == null || placement.unitData == null || placement.unitData.definition == null) continue;

            string unitName = placement.unitData.definition.name;

            if (!metaData.crownedUnits.Contains(unitName))
            {
                metaData.crownedUnits.Add(unitName);
                madeChanges = true;
                Debug.Log($"[Compendium] {unitName} was crowned!");
            }
        }

        if (madeChanges)
        {
            metaData.totalRunsCompleted++;
            SaveManager.Save("meta_save.json", metaData);
        }
    }
}
