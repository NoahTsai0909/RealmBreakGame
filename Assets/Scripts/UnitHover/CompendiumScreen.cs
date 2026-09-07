using UnityEngine;
using System.Collections.Generic;

public class CompendiumScreen : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Drag the Content object from RightContent -> ScrollArea -> Content here")]
    public Transform rightContentGrid;

    [Tooltip("Drag your modified Unit UI Prefab here")]
    public GameObject compendiumCardPrefab;

    [Tooltip("Drag your UnitDetailModal GameObject here")]
    public UnitDetailModal detailModal; // NEW: Added to pass down to the cards

    private List<GameObject> activeCards = new List<GameObject>();

    // This runs automatically every time the player clicks the "Compendium" button
    private void OnEnable()
    {
        PopulateAllUnits();
    }

    public void PopulateAllUnits()
    {
        // 1. Clear any existing cards to prevent duplicates
        foreach (GameObject card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();

        // 2. Fetch every unit in the game from the Database
        List<UnitDefinition> allUnits = UnitDatabase.Instance.allUnits;

        // 3. Generate the UI grid
        foreach (UnitDefinition def in allUnits)
        {
            GameObject newCard = Instantiate(compendiumCardPrefab, rightContentGrid);
            activeCards.Add(newCard);

            CompendiumCardUI cardUI = newCard.GetComponent<CompendiumCardUI>();

            // 4. Check the player's Meta Save Data
            bool isUnlocked = MetaManager.Instance.metaData.unlockedCompendiumUnits.Contains(def.name);
            bool isCrowned = MetaManager.Instance.metaData.crownedUnits.Contains(def.name);

            // 5. Use the single Initialize method!
            cardUI.Initialize(def, isUnlocked, isCrowned, detailModal);
        }
    }

    public void FilterCards(Region? regionFilter, Rarity? rarityFilter, UnitTagFlags tagFilters)
    {
        // Loop through every physical card we spawned
        foreach (GameObject cardObj in activeCards)
        {
            CompendiumCardUI ui = cardObj.GetComponent<CompendiumCardUI>();
            UnitDefinition def = ui.GetDefinition(); // This works now!

            bool matches = true;

            // 1. Check Region
            if (regionFilter != null && def.region != regionFilter) matches = false;

            // 2. Check Rarity
            if (rarityFilter != null && def.startingRarity > rarityFilter) matches = false;

            // 3. Check Tags
            if (tagFilters != UnitTagFlags.None)
            {
                if ((def.tagFlags & tagFilters) != tagFilters)
                {
                    // Special logic: If filtering by Burn, also accept BurnRef!
                    if (tagFilters.HasFlag(UnitTagFlags.Burn) && def.tagFlags.HasFlag(UnitTagFlags.BurnRef))
                        matches = true;
                    else
                        matches = false;
                }
            }

            // Turn the card on or off!
            cardObj.SetActive(matches);
        }
    }
}
