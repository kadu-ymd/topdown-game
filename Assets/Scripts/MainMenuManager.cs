using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject settingsMenuUI;
    public AudioSource buttonClick;

    private void Start()
    {
        settingsMenuUI.SetActive(false);

        if (!PlayerPrefs.HasKey("CurrentUI"))
        {
            PlayerPrefs.SetString("CurrentUI", "MainMenu");
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (PlayerPrefs.GetString("CurrentUI") == "SettingsMenu")
            {
                PlaySound();
                SettingsClose();
            }
        }
        
    }

    public void SettingsOpen()
    {
        settingsMenuUI.SetActive(true);
        PlayerPrefs.SetString("CurrentUI", "SettingsMenu");
    }

    public void SettingsClose()
    {
        if (PlayerPrefs.GetString("CurrentUI") == "SettingsMenu")
        {
            settingsMenuUI.SetActive(false);
            PlayerPrefs.SetString("CurrentUI", "MainMenu");
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("1Bedroom");
        Time.timeScale = 1;
    }

    public void PlaySound()
    {
        if (buttonClick != null)
        {
            buttonClick.Play();
        }
    }
  
}