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
    public UnitDetailModal detailModal;

    private List<GameObject> activeCards = new List<GameObject>();

    private void OnEnable()
    {
        PopulateAllUnits();
    }

    public void PopulateAllUnits()
    {
        foreach (GameObject card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();

        List<UnitDefinition> allUnits = UnitDatabase.Instance.allUnits;

        foreach (UnitDefinition def in allUnits)
        {
            GameObject newCard = Instantiate(compendiumCardPrefab, rightContentGrid);
            activeCards.Add(newCard);

            CompendiumCardUI cardUI = newCard.GetComponent<CompendiumCardUI>();

            bool isUnlocked = MetaManager.Instance.metaData.unlockedCompendiumUnits.Contains(def.name);
            bool isCrowned = MetaManager.Instance.metaData.crownedUnits.Contains(def.name);

            cardUI.Initialize(def, isUnlocked, isCrowned, detailModal);
        }
    }

    public void FilterCards(HashSet<Region> regionFilters, HashSet<Rarity> rarityFilters, UnitTagFlags tagFilters)
    {

        foreach (GameObject cardObj in activeCards)
        {
            CompendiumCardUI ui = cardObj.GetComponent<CompendiumCardUI>();
            UnitDefinition def = ui.GetDefinition();

            bool matches = true;

            if (regionFilters.Count > 0 && !regionFilters.Contains(def.region))
            {
                matches = false;
            }

            if (rarityFilters.Count > 0 && !rarityFilters.Contains(def.startingRarity))
            {
                matches = false;
            }

            if (tagFilters != UnitTagFlags.None)
            {

                UnitTagFlags effectiveTags = def.tagFlags;
                if (effectiveTags.HasFlag(UnitTagFlags.BurnRef)) effectiveTags |= UnitTagFlags.Burn;
                if (effectiveTags.HasFlag(UnitTagFlags.PoisonRef)) effectiveTags |= UnitTagFlags.Poison;
                if (effectiveTags.HasFlag(UnitTagFlags.DamageRef)) effectiveTags |= UnitTagFlags.Damage;
                if (effectiveTags.HasFlag(UnitTagFlags.HealRef)) effectiveTags |= UnitTagFlags.Heal;

                if ((effectiveTags & tagFilters) != tagFilters)
                {
                    matches = false;
                }
            }

            cardObj.SetActive(matches);
        }
    }
}
