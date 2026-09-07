using UnityEngine;
using UnityEngine.UI;

public class UnitDetailModal : MonoBehaviour
{
    [Header("UI References")]
    public Image bigUnitSprite;
    public UnitHoverUI unitHoverUI;

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
        if (activeMannequin != null) Destroy(activeMannequin.gameObject);

        activeMannequin = Instantiate(currentDef.unitPrefab, transform);

        activeMannequin.gameObject.SetActive(false);

        activeMannequin.InitializeEnemy(currentDef, (Rarity)rarityIndex);

        activeMannequin.RecalculateStats();

        unitHoverUI.Show(activeMannequin);
    }

    public void CloseModal()
    {
        gameObject.SetActive(false);
        if (activeMannequin != null) Destroy(activeMannequin.gameObject);
    }
}