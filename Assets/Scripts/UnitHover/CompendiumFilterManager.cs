using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CompendiumFilterManager : MonoBehaviour
{
    public CompendiumScreen compendiumScreen;

    [Header("Active Filters")]
    public Region? selectedRegion = null;
    public Rarity? selectedRarity = null;
    public UnitTagFlags activeTags = UnitTagFlags.None;

    // --- REGION BUTTONS ---
    public void ToggleRegion(int regionIndex)
    {
        Region clickedRegion = (Region)regionIndex;

        // If they click the same region twice, turn it off. Otherwise, swap to it.
        if (selectedRegion == clickedRegion) selectedRegion = null;
        else selectedRegion = clickedRegion;

        ApplyFilters();
    }

    // --- RARITY BUTTONS ---
    public void ToggleRarity(int rarityIndex)
    {
        Rarity clickedRarity = (Rarity)rarityIndex;

        if (selectedRarity == clickedRarity) selectedRarity = null;
        else selectedRarity = clickedRarity;

        ApplyFilters();
    }

    // --- TAG BUTTONS (Multi-Select!) ---
    public void ToggleTag(int tagFlagValue)
    {
        UnitTagFlags clickedTag = (UnitTagFlags)tagFlagValue;

        // Bitwise logic: If they already have the tag, remove it. If not, add it.
        if (activeTags.HasFlag(clickedTag))
            activeTags &= ~clickedTag; // Remove
        else
            activeTags |= clickedTag;  // Add

        ApplyFilters();
    }

    private void ApplyFilters()
    {
        // Tell the main screen to update the visual cards!
        compendiumScreen.FilterCards(selectedRegion, selectedRarity, activeTags);
    }
}
