using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ExitManager : MonoBehaviour {

    public List<string> RequiredItemsName = new List<string>();
    public string nextSceneName;
    private string sceneName;

    private Volume globalVolume;
    private ColorAdjustments colorAdjust;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        sceneName = SceneManager.GetActiveScene().name;
        if (!Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            Debug.LogError("A cena " + nextSceneName + " não existe ou não está incluída na Build Settings.");
            return;
        }
        globalVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
        globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjust);

        foreach (string item in RequiredItemsName) {
            if (!PlayerPrefs.HasKey(item)) {
                PlayerPrefs.SetInt(item, 0);
            }
        }
        PlayerPrefs.Save();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            foreach (string item in RequiredItemsName) {
                if (PlayerPrefs.GetInt(item) == 0) {
                    ThoughtManager.ShowThought("Acho melhor eu vasculhar mais o lugar antes de sair...");
                    return;
                }
            }
            PlayerPrefs.SetInt("TotalDeaths", PlayerPrefs.GetInt("TotalDeaths") + PlayerPrefs.GetInt("Deaths_" + sceneName));
            PlayerPrefs.SetInt("TotalKills", PlayerPrefs.GetInt("TotalKills") + PlayerPrefs.GetInt("Kills_" + sceneName));
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    private IEnumerator FadeOutAndLoadScene() {
        float duration = 1f; 
        float startExposure = colorAdjust.postExposure.value;
        float targetExposure = -10f; // Tom de preto
        float elapsed = 0f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            colorAdjust.postExposure.value = Mathf.Lerp(startExposure, targetExposure, t);
            yield return null;
        }

        colorAdjust.postExposure.value = targetExposure;
        SceneManager.LoadSceneAsync(nextSceneName);
    }
}
