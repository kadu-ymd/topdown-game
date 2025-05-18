using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{  
    private static MenuManager MenuManagerInstance;
    public GameObject menuCanvas;
    private GameObject pauseButton;
    private GameObject pauseMenuUI;
    private GameObject settingsMenuUI;
    private GameObject itensCollected;
    private GameObject bookItem;
    private GameObject gunItem;

    private AudioSource musicSource;
    private Slider musicSlider;

    void Awake() 
    {
        if (MenuManagerInstance == null) MenuManagerInstance = this;
        menuCanvas.SetActive(true);
    }

    void Start()
    {
        GameObject musicPlayer = GameObject.Find("MusicPlayer");

        pauseButton = menuCanvas.transform.Find("PauseButton")?.gameObject;
        pauseMenuUI = menuCanvas.transform.Find("PauseMenu")?.gameObject;
        settingsMenuUI = menuCanvas.transform.Find("SettingsMenu")?.gameObject;

        itensCollected = menuCanvas.transform.Find("ItensCollected")?.gameObject;
        bookItem = itensCollected.transform.Find("Book")?.gameObject;
        gunItem = itensCollected.transform.Find("Gun")?.gameObject;


        ActivePauseButton(true);
        ActivePauseMenuUI(false);
        ActiveSettingsMenuUI(false);
        itensCollected.SetActive(true);

        if (!PlayerPrefs.HasKey("CurrentUI"))
        {
            PlayerPrefs.SetString("CurrentUI", "None");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string currentMenu = PlayerPrefs.GetString("CurrentUI");
            if (currentMenu == "None")
            {
                PauseGame();
                currentMenu = "PauseMenu";
            }
            else if (currentMenu == "PauseMenu")
            {
                ResumeGame();
                currentMenu = "None";
            }
            else if (currentMenu == "SettingsMenu")
            {
                SettingsClose();
                currentMenu = "PauseMenu";

            }
        }

        UpdateItensCollectedDisplay();
    }

    private void UpdateItensCollectedDisplay()
    {
    if (bookItem != null)
        bookItem.SetActive(PlayerPrefs.GetInt("Book", 0) > 0);

    if (gunItem != null)
        gunItem.SetActive(PlayerPrefs.GetInt("Gun", 0) > 0);
    }

    private void ActivePauseButton(bool show)
    {
        if (pauseButton != null)
        {
            pauseButton.SetActive(show);
        }
    }
    private void ActivePauseMenuUI(bool show)
    {
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(show);
        }
    }
    private void ActiveSettingsMenuUI(bool show)
    {
        if (settingsMenuUI != null)
        {
            settingsMenuUI.SetActive(show);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        ActivePauseButton(true);
        ActivePauseMenuUI(false);
        PlayerPrefs.SetString("CurrentUI", "None");
    }

    public void SettingsOpen()
    {
        ActivePauseMenuUI(false);
        ActiveSettingsMenuUI(true);
        PlayerPrefs.SetString("CurrentUI", "SettingsMenu");
    }

    public void SettingsClose()
    {
        ActiveSettingsMenuUI(false);
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            PlayerPrefs.SetString("CurrentUI", "PauseMenu");
        } 
        else 
        {
            PlayerPrefs.SetString("CurrentUI", "None");
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        ActivePauseButton(false);
        ActivePauseMenuUI(true);
        PlayerPrefs.SetString("CurrentUI", "PauseMenu");

        UpdateItensCollectedDisplay();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("1Bedroom");
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public static void ShowPauseButton() 
    {
        MenuManagerInstance.ActivePauseButton(true);
    }
    public static void HidePauseButton() 
    {
        MenuManagerInstance.ActivePauseButton(false);
    }
}
