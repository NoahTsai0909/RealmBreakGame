using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
public static class SaveLoadManager
{
    public static bool pendingLoad = false;
    private static string SavePath => Application.persistentDataPath + "/midgame_save.json";

    // ==========================================
    // 1. SAVE THE RUN
    // ==========================================
    public static void SaveRun()
    {
        RunManager rm = RunManager.Instance;
        if (rm == null) return;

        RunSaveData data = new RunSaveData();

        // Save Primitives & Stats
        data.stats = rm.Stats;
        data.playerRegion = rm.playerRegion;
        data.totalDays = rm.TOTAL_DAYS;
        data.regularEventsCompleted = rm.regularEventsCompleted;
        data.isBattlePhase = rm.isBattlePhase;
        data.currentEventPhase = rm.currentEventPhase;
        data.hasUsedLastChance = rm.hasUsedLastChance;

        // Save Dictionaries (Newtonsoft handles this automatically!)
        data.permanentStatsMap = rm.GetPermanentStatsMap(); // *Note: See minor tweak below*
        data.masterUnitStats = rm.masterUnitStats;

        // Save Placements (Mapping ScriptableObjects to Strings)
        data.playerTeam = ConvertUnitPlacementsToDTO(rm.playerTeamPlacements);
        data.playerBench = ConvertUnitPlacementsToDTO(rm.playerBenchPlacements);
        data.playerTactics = ConvertTacticPlacementsToDTO(rm.playerTactics);

        // Save Events
        data.allDayEventNames = rm.allDayEvents.Select(e => e.name).ToList();
        data.currentDailyEventNames = rm.currentDailyEvents.Select(e => e.name).ToList();
        if (rm.selectedEvent != null) data.selectedEventName = rm.selectedEvent.name;

        // Write to Hard Drive
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(SavePath, json);

        Debug.Log($"Game Saved Successfully to: {SavePath}");
    }

    // ==========================================
    // 2. LOAD THE RUN
    // ==========================================
    public static bool LoadRun()
    {
        if (!File.Exists(SavePath)) return false;

        try
        {
            string json = File.ReadAllText(SavePath);
            RunSaveData data = JsonConvert.DeserializeObject<RunSaveData>(json);
            RunManager rm = RunManager.Instance;

            // Restore Primitives & Stats
            rm.Stats = data.stats;
            rm.playerRegion = data.playerRegion;
            rm.AssignRegionTree();
            rm.TOTAL_DAYS = data.totalDays;
            rm.regularEventsCompleted = data.regularEventsCompleted;
            rm.isBattlePhase = data.isBattlePhase;
            rm.currentEventPhase = data.currentEventPhase;
            rm.hasUsedLastChance = data.hasUsedLastChance;

            // SAFELY RESTORE DICTIONARIES (No '??' operator)
            if (data.permanentStatsMap != null)
                rm.SetPermanentStatsMap(data.permanentStatsMap);
            else
                rm.SetPermanentStatsMap(new Dictionary<System.Guid, PermanentStats>());

            if (data.masterUnitStats != null)
                rm.masterUnitStats = data.masterUnitStats;
            else
                rm.masterUnitStats = new Dictionary<System.Guid, UnitLifetimeStats>();

            // Restore Placements (with safety checks in case Newtonsoft skipped empty lists)
            if (data.playerTeam != null)
                rm.playerTeamPlacements = RestoreUnitPlacements(data.playerTeam);

            if (data.playerBench != null)
                rm.playerBenchPlacements = RestoreUnitPlacements(data.playerBench);

            // Dynamically pad the bench to exactly match the inspector size!
            while (rm.playerBenchPlacements.Count < rm.benchSize)
            {
                rm.playerBenchPlacements.Add(new RunManager.UnitPlacement { row = -1, col = -1 });
            }

            if (data.playerTactics != null)
                rm.playerTactics = RestoreTacticPlacements(data.playerTactics);

            BaseEventSO[] allAvailableEvents = Resources.LoadAll<BaseEventSO>("Events");

            if (data.allDayEventNames != null)
            {
                rm.allDayEvents = data.allDayEventNames
                    .Select(name => allAvailableEvents.FirstOrDefault(e => e.name == name))
                    .Where(e => e != null).ToList();
            }

            // NEW: Restore the 3 active choices!
            if (data.currentDailyEventNames != null)
            {
                rm.currentDailyEvents = data.currentDailyEventNames
                    .Select(name => allAvailableEvents.FirstOrDefault(e => e.name == name))
                    .Where(e => e != null).ToList();
            }

            if (!string.IsNullOrEmpty(data.selectedEventName))
                rm.selectedEvent = allAvailableEvents.FirstOrDefault(e => e.name == data.selectedEventName);

            Debug.Log("Game Loaded Successfully!");
            return true;
        }
        catch (System.Exception e)
        {
            // IF ANYTHING FAILS, WE WILL SEE EXACTLY WHY!
            Debug.LogError($"Save file crashed during loading! Error: {e.Message}\n{e.StackTrace}");
            return false;
        }
    }

    // ==========================================
    // HELPER METHODS (Data Mapping)
    // ==========================================
    private static List<UnitPlacementDTO> ConvertUnitPlacementsToDTO(List<RunManager.UnitPlacement> placements)
    {
        List<UnitPlacementDTO> dtos = new List<UnitPlacementDTO>();
        foreach (var p in placements)
        {
            if (p.unitData == null || p.unitData.definition == null) continue;

            dtos.Add(new UnitPlacementDTO
            {
                row = p.row,
                col = p.col,
                unitDefinitionName = p.unitData.definition.name,
                rarity = p.unitData.rarity,
                prefixName = p.unitData.prefix != null ? p.unitData.prefix.name : null,
                suffixName = p.unitData.suffix != null ? p.unitData.suffix.name : null,
                id = p.unitData.id // NEW: Save the ID
            });
        }
        return dtos;
    }

    private static List<RunManager.UnitPlacement> RestoreUnitPlacements(List<UnitPlacementDTO> dtos)
    {
        List<RunManager.UnitPlacement> placements = new List<RunManager.UnitPlacement>();
        MutationPrefixSO[] allPrefixes = Resources.LoadAll<MutationPrefixSO>("");
        MutationSuffixSO[] allSuffixes = Resources.LoadAll<MutationSuffixSO>("");

        foreach (var dto in dtos)
        {
            UnitDefinition def = UnitDatabase.Instance.allUnits.FirstOrDefault(u => u.name == dto.unitDefinitionName);
            if (def == null) continue;

            // NEW: Inject the saved ID so it remembers its permanent stats!
            UnitSaveData data = new UnitSaveData { definition = def, rarity = dto.rarity, id = dto.id };

            if (!string.IsNullOrEmpty(dto.prefixName))
                data.prefix = allPrefixes.FirstOrDefault(p => p.name == dto.prefixName);
            if (!string.IsNullOrEmpty(dto.suffixName))
                data.suffix = allSuffixes.FirstOrDefault(s => s.name == dto.suffixName);

            placements.Add(new RunManager.UnitPlacement { row = dto.row, col = dto.col, unitData = data });
        }
        return placements;
    }

    private static List<TacticPlacementDTO> ConvertTacticPlacementsToDTO(List<RunManager.TacticPlacement> tactics)
    {
        return tactics.Where(t => t.tacticData != null && t.tacticData.definition != null).Select(t => new TacticPlacementDTO
        {
            orderIndex = t.orderIndex,
            tacticDefinitionName = t.tacticData.definition.name,
            rarity = t.tacticData.rarity,
            id = t.tacticData.id
        }).ToList();
    }

    private static List<RunManager.TacticPlacement> RestoreTacticPlacements(List<TacticPlacementDTO> dtos)
    {
        List<RunManager.TacticPlacement> tactics = new List<RunManager.TacticPlacement>();
        foreach (var dto in dtos)
        {
            TacticDefinition def = TacticDatabase.Instance.allTactics.FirstOrDefault(t => t.name == dto.tacticDefinitionName);
            if (def == null) continue;

            RunManager.TacticSaveData data = new RunManager.TacticSaveData { definition = def, rarity = dto.rarity, id = dto.id };
            tactics.Add(new RunManager.TacticPlacement { orderIndex = dto.orderIndex, tacticData = data });
        }
        return tactics;
    }

    public static void DeleteSave()
    {
        if (File.Exists(SavePath)) File.Delete(SavePath);
    }

    public static bool HasSaveFile() => File.Exists(SavePath);
}