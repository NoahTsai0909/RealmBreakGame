using UnityEngine;

public class ProvisionManager : MonoBehaviour
{
    [SerializeField] private GridManager battleGrid;
    [SerializeField] private GridManager benchGrid;
    private RunManager runManager;

    private int currentProvisionUsed = 0;

    void Start()
    {
        if (runManager == null)
            runManager = RunManager.Instance;

        CalculateCurrentProvision();
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (RunHUDManager.Instance != null)
        {
            RunHUDManager.Instance.ClearCurrentProvision();
        }
    }

    public int GetUnitProvisionCost(UnitInstance unit)
    {
        if (unit == null || unit.myPlacement == null || unit.myPlacement.unitData == null)
            return 0;

        return unit.myPlacement.unitData.provisionCost > 0
            ? unit.myPlacement.unitData.provisionCost
            : unit.Definition.provisionCost;
    }

    public int CalculateProvisionForGrid(GridManager grid)
    {
        int total = 0;
        var units = grid.GetAllUnits();

        foreach (var unit in units)
        {
            total += GetUnitProvisionCost(unit);
        }

        return total;
    }

    public void CalculateCurrentProvision()
    {
        currentProvisionUsed = CalculateProvisionForGrid(battleGrid);
        UpdateUI();
    }

    public bool CanAddUnitToBattleGrid(UnitInstance unit)
    {
        int unitCost = GetUnitProvisionCost(unit);
        int projectedTotal = currentProvisionUsed + unitCost;

        return projectedTotal <= runManager.Stats.ProvisionCap;
    }

    public bool CanSwapUnits(UnitInstance unitLeavingBattle, UnitInstance unitEnteringBattle)
    {
        int leavingCost = GetUnitProvisionCost(unitLeavingBattle);
        int enteringCost = GetUnitProvisionCost(unitEnteringBattle);
        int netChange = enteringCost - leavingCost;

        return currentProvisionUsed + netChange <= runManager.Stats.ProvisionCap;
    }

    public bool IsProvisionValid()
    {
        return currentProvisionUsed <= runManager.Stats.ProvisionCap;
    }

    private void UpdateUI()
    {
        if (RunHUDManager.Instance != null)
        {
            RunHUDManager.Instance.SetCurrentProvision(currentProvisionUsed, IsProvisionValid());
        }
    }

    public void OnUnitMoved(GridManager fromGrid, GridManager toGrid, UnitInstance unit)
    {
        CalculateCurrentProvision();
    }

    public void HideProvisionText()
    {
        if (RunHUDManager.Instance != null)
        {
            RunHUDManager.Instance.ClearCurrentProvision();
        }
    }
}
