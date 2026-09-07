using UnityEngine;
using UnityEngine.UI;

public class UnitDetailModal : MonoBehaviour
{
    [Header("UI References")]
    public Image bigUnitSprite;
    public UnitHoverUI unitHoverUI;

    // We no longer need the DummyMannequin prefab slot!
    // We will spawn the real unit directly from the database.

    [Header("Rarity Buttons (In Order: Com, Unc, Rare, Epic)")]
    public Button[] rarityButtons;

    private UnitDefinition currentDef;
    private UnitInstance activeMannequin;

    public void OpenModal(UnitDefinition def)
    {
        currentDef = def;
        gameObject.SetActive(true);
        bigUnitSprite.sprite = def.unitSprite;

        int startRarity = (int)def.startingRarity;
        for (int i = 0; i < rarityButtons.Length; i++)
        {
            rarityButtons[i].gameObject.SetActive(i >= startRarity);
        }

        ShowRarity(startRarity);
    }

    public void ShowRarity(int rarityIndex)
    {
        // 1. Destroy the old preview if we are switching rarities or units
        if (activeMannequin != null) Destroy(activeMannequin.gameObject);

        // 2. Instantiate the ACTUAL unit prefab (e.g., the real Duelist) so we get its abilities!
        activeMannequin = Instantiate(currentDef.unitPrefab, transform);

        // Hide its physical body so only the UI shows
        activeMannequin.gameObject.SetActive(false);

        // 3. IMPORTANT: Use InitializeEnemy! 
        // InitializeFromSaveData crashes in the Main Menu because RunManager doesn't exist.
        activeMannequin.InitializeEnemy(currentDef, (Rarity)rarityIndex);

        // Force the stats to calculate while it is asleep
        activeMannequin.RecalculateStats();

        // 4. Show the UI!
        unitHoverUI.Show(activeMannequin);
    }

    public void CloseModal()
    {
        gameObject.SetActive(false);
        if (activeMannequin != null) Destroy(activeMannequin.gameObject);
    }
}