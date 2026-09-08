using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro; // Needed for the InputField

public class CompendiumFilterManager : MonoBehaviour
{
    public CompendiumScreen compendiumScreen;

    [Header("Active Filters")]
    public HashSet<Region> selectedRegions = new HashSet<Region>();
    public HashSet<Rarity> selectedRarities = new HashSet<Rarity>();
    public UnitTagFlags activeTags = UnitTagFlags.None;

    [Header("Search")]
    public GameObject searchBarContainer;
    public TMP_InputField searchInputField;
    private string searchQuery = "";

    public void ToggleSearchBar()
    {
        if (searchBarContainer != null)
        {
            bool isActive = !searchBarContainer.activeSelf;
            searchBarContainer.SetActive(isActive);

            if (isActive)
            {
                searchInputField.Select();
                searchInputField.ActivateInputField();
            }
            else
            {
                searchInputField.text = "";
                UpdateSearchQuery("");
            }
        }
    }

    public void UpdateSearchQuery(string query)
    {
        searchQuery = query.ToLower();
        ApplyFilters();
    }

    public void ToggleRegion(int regionIndex)
    {
        Region clickedRegion = (Region)regionIndex;
        if (selectedRegions.Contains(clickedRegion)) selectedRegions.Remove(clickedRegion);
        else selectedRegions.Add(clickedRegion);

        GameObject clickedObj = EventSystem.current.currentSelectedGameObject;
        if (clickedObj != null && clickedObj.transform.parent != null) UpdateRegionVisuals(clickedObj.transform.parent);

        ApplyFilters();
    }

    private void UpdateRegionVisuals(Transform regionGrid)
    {
        foreach (Transform child in regionGrid)
        {
            Image icon = child.GetComponent<Image>();
            if (icon == null) continue;

            bool isSelected = false;
            if (selectedRegions.Count == 0) isSelected = true;
            else
            {
                if (child.name.Contains("Solmire") && selectedRegions.Contains(Region.Solmire)) isSelected = true;
                if (child.name.Contains("Nethervale") && selectedRegions.Contains(Region.Nethervale)) isSelected = true;
                if (child.name.Contains("Everborn") && selectedRegions.Contains(Region.Everborn)) isSelected = true;
                if (child.name.Contains("Axiom") && selectedRegions.Contains(Region.Axiom)) isSelected = true;
            }

            if (isSelected)
            {
                icon.color = Color.white;
                child.localScale = new Vector3(1.15f, 1.15f, 1f);
            }
            else
            {
                icon.color = new Color(0.25f, 0.25f, 0.25f, 1f);
                child.localScale = new Vector3(0.85f, 0.85f, 1f);
            }
        }
    }

    public void ToggleRarity(int rarityIndex)
    {
        Rarity clickedRarity = (Rarity)rarityIndex;
        bool isNowSelected = false;

        if (selectedRarities.Contains(clickedRarity)) selectedRarities.Remove(clickedRarity);
        else
        {
            selectedRarities.Add(clickedRarity);
            isNowSelected = true;
        }

        ToggleGlow(isNowSelected);
        ApplyFilters();
    }

    public void ToggleTag(int tagFlagValue)
    {
        UnitTagFlags clickedTag = (UnitTagFlags)tagFlagValue;
        bool isNowSelected = false;

        if (activeTags.HasFlag(clickedTag)) activeTags &= ~clickedTag;
        else
        {
            activeTags |= clickedTag;
            isNowSelected = true;
        }

        ToggleGlow(isNowSelected);
        ApplyFilters();
    }

    private void ToggleGlow(bool isSelected)
    {
        GameObject clickedObj = EventSystem.current.currentSelectedGameObject;
        if (clickedObj == null) return;

        Outline glow = clickedObj.GetComponent<Outline>();
        if (glow == null)
        {
            glow = clickedObj.AddComponent<Outline>();
            glow.effectColor = new Color(1f, 0.8f, 0.2f, 1f);
            glow.effectDistance = new Vector2(3, -3);
        }
        glow.enabled = isSelected;
    }

    private void ApplyFilters()
    {
        compendiumScreen.FilterCards(selectedRegions, selectedRarities, activeTags, searchQuery);
    }
}