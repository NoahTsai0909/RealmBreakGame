using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SceneLoader;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button compendiumButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject compendiumPanel;
    [SerializeField] private GameObject LeftSideBar;
    [SerializeField] private GameObject FilterSideBar;
    [SerializeField] private Button filterButton;
    [SerializeField] private Button searchButton;
    [SerializeField] private Button closeCompendiumButton;
    
    private bool isFilterSidebarOpen = false;
    void Start()
    {
        Application.runInBackground = true; // Prevents pausing when tabbed out

        playButton.onClick.AddListener(() => SceneLoader.Instance.LoadScene(GameScene.AdventureSelectionScene));
        compendiumButton.onClick.AddListener(() => ShowCompendium());
        settingsButton.onClick.AddListener(() => Debug.Log("Settings coming soon!"));
        filterButton.onClick.AddListener(() => ToggleFilterSideBar());
    }



    void ShowCompendium()
    {
        mainMenuPanel.SetActive(false);
        compendiumPanel.SetActive(true);
        closeCompendiumButton.onClick.AddListener(() => CloseCompendium());
    }

    void CloseCompendium()
    {
        compendiumPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    void ToggleFilterSideBar()
    {
        isFilterSidebarOpen = !isFilterSidebarOpen;
        FilterSideBar.SetActive(isFilterSidebarOpen);
        LeftSideBar.SetActive(!isFilterSidebarOpen);
    }
}
