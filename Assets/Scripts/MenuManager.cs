using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [HideInInspector] public static MenuManager MenuManagerInstance;
    public GameObject menuCanvas;
    private GameObject pauseButton;
    private GameObject hintButton;
    private GameObject pauseMenuUI;
    private GameObject settingsMenuUI;
    public GameObject bookButton;
    public GameObject gunButton;
    public GameObject joyStick;
    public GameObject interactButton;


    void Awake()
    {
        if (MenuManagerInstance == null) MenuManagerInstance = this;
        menuCanvas.SetActive(true);
    }

    void Start()
    {
        GameObject musicPlayer = GameObject.Find("MusicPlayer");

        pauseButton = menuCanvas.transform.Find("PauseButton")?.gameObject;
        hintButton = menuCanvas.transform.Find("HintButton")?.gameObject;
        pauseMenuUI = menuCanvas.transform.Find("PauseMenu")?.gameObject;
        settingsMenuUI = menuCanvas.transform.Find("SettingsMenu")?.gameObject;


        ActivePauseButton(true);
        ActiveInteractButton(false);
        ActivePauseMenuUI(false);
        ActiveSettingsMenuUI(false);

        if (!PlayerPrefs.HasKey("CurrentUI"))
        {
            PlayerPrefs.SetString("CurrentUI", "None");
        }
    }

    void Update()
    {
        ActiveActionButtons(PlayerPrefs.GetString("CurrentUI") == "None");
    }

    private void ActiveActionButtons(bool show)
    {
        if (bookButton != null)
            bookButton.SetActive(show && PlayerPrefs.GetInt("Book", 0) > 0);

        if (gunButton != null)
            gunButton.SetActive(show && PlayerPrefs.GetInt("Gun", 0) > 0);

        if (joyStick != null)
            joyStick.SetActive(show);

        if (interactButton != null && !show)
            interactButton.SetActive(show);
    }

    private void ActivePauseButton(bool show)
    {
        if (pauseButton != null)
            pauseButton.SetActive(show);
    }

    private void ActiveHintButton(bool show)
    {
        if (hintButton != null)
            hintButton.SetActive(show);
    }

    private void ActivePauseMenuUI(bool show)
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(show);
    }
    private void ActiveSettingsMenuUI(bool show)
    {
        if (settingsMenuUI != null)
            settingsMenuUI.SetActive(show);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        ActivePauseButton(true);
        ActiveHintButton(true);
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
        ActiveHintButton(false);
        ActivePauseMenuUI(true);
        PlayerPrefs.SetString("CurrentUI", "PauseMenu");

        ActiveActionButtons(true);
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
        MenuManagerInstance.ActiveHintButton(true);
        MenuManagerInstance.ActiveActionButtons(true);
    }
    public static void HidePauseButton()
    {
        MenuManagerInstance.ActivePauseButton(false);
        MenuManagerInstance.ActiveHintButton(false);
        MenuManagerInstance.ActiveActionButtons(false);
    }
    
    public static void ActiveInteractButton(bool active)
    {
        if (MenuManagerInstance.interactButton != null)
            MenuManagerInstance.interactButton.SetActive(active);
        
    }
}
