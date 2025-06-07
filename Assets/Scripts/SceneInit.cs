using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;

public class SceneInit : MonoBehaviour {

    protected static bool Initialized = false;
    protected List<string> AllCollectables = new List<string> {
        "Book", "Gun",
        "FirstMemory", "SecondMemory",
        "PharmCard", "MedicKey", "StaffKey"
    };
    protected string sceneName;
    public List<string> currentRequiredPlayerItems = new List<string>();
    public MonoBehaviour runOnInit;
    public bool canRevive = false;
    private bool isInitialScene = true; // Começa como true para a primeira execução
    private Volume globalVolume;
    private ColorAdjustments colorAdjust;
    private Vignette vignette;
    private float targetExposure;

    protected void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "1Bedroom")
            PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("LastScene"))
            PlayerPrefs.SetString("LastScene", sceneName);
        
        if (!Initialized || PlayerPrefs.GetString("LastScene") != sceneName)
            {
                foreach (string collectable in AllCollectables)
                {
                    if (currentRequiredPlayerItems.Contains(collectable))
                        PlayerPrefs.SetInt(collectable, 1);
                    else
                        PlayerPrefs.SetInt(collectable, 0);
                }


                if (!PlayerPrefs.HasKey("TotalDeaths"))
                    PlayerPrefs.SetInt("TotalDeaths", 0);

                if (!PlayerPrefs.HasKey("TotalKills"))
                    PlayerPrefs.SetInt("TotalKills", 0);

                if (!PlayerPrefs.HasKey("Deaths_" + sceneName))
                    PlayerPrefs.SetInt("Deaths_" + sceneName, 0);

                PlayerPrefs.SetInt("CanRevive", canRevive ? 1 : 0);
                PlayerPrefs.SetString("LastScene", sceneName);
                Initialized = true;

            }
        PlayerPrefs.SetString("CurrentUI", "None");
        PlayerPrefs.SetInt($"Kills_" + sceneName, 0);
    }

    protected void Start()
    {
        BookManager.UpdatedeBookPages();

        globalVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
        globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjust);
        targetExposure = colorAdjust.postExposure.value;
        colorAdjust.postExposure.value = -10f;
        StartCoroutine(FadeIn());
        if (isInitialScene) {
            GameObject postProcessingObject = GameObject.Find("PostProcessingVolume");
            if (postProcessingObject != null){
                var volume = postProcessingObject.GetComponent<Volume>();            
                volume.profile.TryGet<Vignette>(out vignette);
                StartCoroutine(WakingUpEffect());
            }
        }
    }


    protected virtual void RunInitialization()
    {
        if (runOnInit != null)
            runOnInit.Invoke("Run", 0); // Dispara ThoughtFlowManager.Run()
    }

    private IEnumerator WakingUpEffect()
    {
        float duration = 4f; // Tempo total do efeito
        float etapa1 = duration * 0.4f; // Primeira etapa: abertura parcial
        float etapa2 = duration * 0.3f; // Segunda etapa: fechar parcialmente
        float etapa3 = duration * 0.3f; // Terceira etapa: abrir completamente

        vignette.intensity.value = 1; // Começa com tudo preto

        // Etapa 1: abertura parcial
        float elapsed = 0f;
        while (elapsed < etapa1)
        {
            elapsed += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(1, 0.5f, elapsed / etapa1); // Vai de 1 -> 0.5
            yield return null;
        }

        // Etapa 2: fechar parcialmente
        elapsed = 0f;
        while (elapsed < etapa2)
        {
            elapsed += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(0.5f, 0.8f, elapsed / etapa2); // Vai de 0.5 -> 0.8
            yield return null;
        }

        // Etapa 3: abrir completamente
        elapsed = 0f;
        while (elapsed < etapa3)
        {
            elapsed += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(0.8f, 0, elapsed / etapa3); // Vai de 0.8 -> 0
            yield return null;
        }

        vignette.intensity.value = 0; // Garante que fique completamente transparente
        isInitialScene = false;
        RunInitialization();
    }

    private IEnumerator FadeIn()
    {
        float duration = 1f;
        float startExposure = colorAdjust.postExposure.value; // já será -10
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            colorAdjust.postExposure.value = Mathf.Lerp(startExposure, targetExposure, t);
            yield return null;
        }

        colorAdjust.postExposure.value = targetExposure;
        RunInitialization();
    }
}