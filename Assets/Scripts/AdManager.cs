using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdManager : MonoBehaviour
{
    private static AdManager AdManagerInstance;

    public GameObject adCanvas;
    public GameObject closeButton;
    public Slider timerSlider;

    public float adDuration = 15f;
    public float skipTime = 5f;

    void Awake()
    {
        if (AdManagerInstance == null) AdManagerInstance = this;
        adCanvas.SetActive(false);
    }

    public static void OpenAd()
    {
        Time.timeScale = 0f;
        PlayerPrefs.SetString("CurrentUI", "Ad");
        AdManagerInstance.adCanvas.SetActive(true);
        AdManagerInstance.closeButton.SetActive(false);
        AdManagerInstance.StartCoroutine(AdManagerInstance.Ad());
        MenuManager.HidePauseButton(); 
    }
    public void CloseAd()
    {
        Time.timeScale = 1f;
        adCanvas.SetActive(false);
        PlayerPrefs.SetString("CurrentUI", "None");
        MenuManager.ShowPauseButton();
    }
    IEnumerator Ad()
    {
        float elapsedTime = 0f;
        timerSlider.value = 0f;

        while (elapsedTime < adDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            timerSlider.value = Mathf.Clamp01(elapsedTime / adDuration);
            if (!closeButton.activeSelf && elapsedTime >= skipTime)
                closeButton.SetActive(true);
            yield return new WaitForEndOfFrame();
        }
        
        if (PlayerPrefs.GetString("CurrentUI") == "Ad")
            CloseAd();
    }
}
