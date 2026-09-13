using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LootSummaryUI : MonoBehaviour
{
    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI xpText;

    [Header("Reward Display")]
    [SerializeField] private Transform rewardAnchor;
    [SerializeField] private Button claimButton;
    [SerializeField] private Button rejectButton;
    [SerializeField] private GameObject rewardContainer;

    private UnitDefinition pendingUnit;
    private Rarity pendingUnitRarity;
    private MutationPrefixSO pendingPrefix;
    private MutationSuffixSO pendingSuffix;


    private TacticDefinition pendingTactic;
    private Rarity pendingTacticRarity;

    private UnitInstance spawnedUnitPreview;
    private TacticInstance spawnedTacticPreview;

    public void ShowSummary(int gold, int xp, UnitDefinition unitDef, Rarity uRarity, TacticDefinition tacticDef, Rarity tRarity, MutationPrefixSO prefix = null, MutationSuffixSO suffix = null)
    {
        gameObject.SetActive(true);
        goldText.SetText(TextIconUtility.ParseDescription($"+ [GOLD] {gold}"));
        xpText.text = $"+{xp} XP";

        pendingUnit = unitDef;
        pendingUnitRarity = uRarity;
        pendingPrefix = prefix; 
        pendingSuffix = suffix; 

        pendingTactic = tacticDef;
        pendingTacticRarity = tRarity;

        ClearPreviews();

        if (pendingUnit != null || pendingTactic != null)
        {
            rewardContainer.SetActive(true);

            if (pendingUnit != null)
                SpawnUnitPreview(pendingUnit, pendingUnitRarity, pendingPrefix, pendingSuffix); 
            else if (pendingTactic != null)
                SpawnTacticPreview(pendingTactic, pendingTacticRarity);

            claimButton.onClick.RemoveAllListeners();
            claimButton.onClick.AddListener(AcceptReward);

            rejectButton.onClick.RemoveAllListeners();
            rejectButton.onClick.AddListener(CloseSummary);
        }
        else
        {
            rewardContainer.SetActive(false);
        }
    }

    private void SpawnUnitPreview(UnitDefinition def, Rarity rarity, MutationPrefixSO prefix, MutationSuffixSO suffix)
    {
        spawnedUnitPreview = Instantiate(def.unitPrefab, rewardAnchor);

        UnitSaveData mockData = new UnitSaveData
        {
            definition = def,
            rarity = rarity,
            prefix = prefix,
            suffix = suffix
        };
        spawnedUnitPreview.InitializeFromSaveData(mockData);

        spawnedUnitPreview.isPlayer = true;
        spawnedUnitPreview.enabled = false; 

        spawnedUnitPreview.transform.localPosition = Vector3.zero;
        spawnedUnitPreview.transform.localScale = Vector3.one * 30f;

        if (spawnedUnitPreview.Visuals != null)
            spawnedUnitPreview.Visuals.SetBaseScale(spawnedUnitPreview.transform.localScale);

        if (spawnedUnitPreview.Visuals != null)
        {
            spawnedUnitPreview.Visuals.SetBaseScale(spawnedUnitPreview.transform.localScale);
            spawnedUnitPreview.Visuals.SyncSortingOrder(100);
        }
    }

    private void SpawnTacticPreview(TacticDefinition def, Rarity rarity)
    {
        spawnedTacticPreview = Instantiate(def.tacticPrefab, rewardAnchor);

        RunManager.TacticSaveData mockData = new RunManager.TacticSaveData { definition = def, rarity = rarity };
        spawnedTacticPreview.InitializeFromSaveData(mockData);

        spawnedTacticPreview.isPlayer = true;
        spawnedTacticPreview.enabled = false;

        spawnedTacticPreview.transform.localPosition = Vector3.zero;
        spawnedTacticPreview.transform.localScale = Vector3.one * 50f;

        Canvas canvas = spawnedTacticPreview.GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;
        }
    }

    private void AcceptReward()
    {
        if (pendingUnit != null)
        {
            PlayerUnitManager.Instance.TryAcquireUnit(pendingUnit, pendingUnitRarity, pendingPrefix, pendingSuffix);
        }
        else if (pendingTactic != null)
            PlayerTacticManager.Instance.TryAcquireTactic(pendingTactic, pendingTacticRarity);

        CloseSummary();
    }

    private void CloseSummary()
    {
        ClearPreviews();
        gameObject.SetActive(false);
    }

    private void ClearPreviews()
    {
        if (spawnedUnitPreview != null) Destroy(spawnedUnitPreview.gameObject);
        if (spawnedTacticPreview != null) Destroy(spawnedTacticPreview.gameObject);
    }
}