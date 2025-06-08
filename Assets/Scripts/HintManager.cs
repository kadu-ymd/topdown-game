using Unity.VisualScripting;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    public GameObject adDecisionMakerUI;    // Display para decidir se quer assistir ou não o ad
    public AudioSource buttonClick;         // Som de clique do botão
    private GameObject pauseButton;
    private GameObject hintButton;
    public GameObject menuCanvas;
    public GameObject hintCanvas;
    private bool hintFlag;

    private void Start()
    {
        hintFlag = false;
        hintCanvas.SetActive(false);
        adDecisionMakerUI.SetActive(false); // Garante que a UI esteja oculta ao iniciar a cena
        pauseButton = menuCanvas.transform.Find("PauseButton")?.gameObject;
        hintButton = menuCanvas.transform.Find("HintButton")?.gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) // Ao apertar ESC, o display fecha
        {
            if (adDecisionMakerUI.activeSelf)
            {
                PlaySound();
                CloseDisplay();
            }
        }

        if (PlayerPrefs.GetString("CurrentUI") == "None" && hintFlag)
        {
            ActiveHintCanvas(hintFlag);
            hintFlag = false;
        }
    }

    private void ActiveHintCanvas(bool show)
    {
        if (hintCanvas != null)
        {
            hintCanvas.SetActive(show);
        }
    }

    private void ActiveHintButton(bool show)
    {
        if (hintButton != null)
        {
            hintButton.SetActive(show);
        }
    }

    private void ActivePauseButton(bool show)
    {
        if (pauseButton != null)
        {
            pauseButton.SetActive(show);
        }
    }

    public void PlaySound()
    {
        if (buttonClick != null)
        {
            buttonClick.Play(); // Toca o som de clique do botão
        }
    }

    public void CloseDisplay()
    {
        ActiveHintButton(true);
        ActivePauseButton(true);
        adDecisionMakerUI.SetActive(false);         // Fecha o display
        PlayerPrefs.SetString("CurrentUI", "None"); // Reseta o PlayerPref
        Time.timeScale = 1f;                        // Faz com que o jogo volte a rodar normalmente
    }

    public void OpenDisplay()
    {
        if (adDecisionMakerUI != null)
        {
            ActiveHintButton(false);
            ActivePauseButton(false);
            adDecisionMakerUI.SetActive(true);                      // Abre o display
            Time.timeScale = 0f;                                    // Pausa o jogo
            PlayerPrefs.SetString("CurrentUI", "AdDecisionMaker");  // Atribui o valor correto ao PlayerPref
        }
        else
        {
            Debug.LogWarning("adDecisionMakerUI is not assigned in the HintManager."); // Log para debug
        }
    }

    public void PlayAd()
    {
        PlaySound();                                    // Toca o som de clique do botão
        adDecisionMakerUI.SetActive(false);             // Fecha a UI de decisão
        AdManager.OpenAd();

        hintFlag = true;
    }

    public void CloseHintDisplay()
    {
        PlaySound();
        ActiveHintCanvas(hintFlag);
        PlayerPrefs.SetString("CurrentUI", "None");
        ActiveHintButton(true);
        ActivePauseButton(true);
        Time.timeScale = 1f;
    }
}
