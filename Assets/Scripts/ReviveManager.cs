using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ReviveManager : MonoBehaviour
{
    private static ReviveManager ReviveManagerInstance;

    public GameObject reviveCanvas;
    public Slider timerSlider;

    public float proposalDuration = 20f;
    private Coroutine proposalCoroutine;

    void Awake()
    {
        if (ReviveManagerInstance == null) ReviveManagerInstance = this;
        reviveCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string currentMenu = PlayerPrefs.GetString("CurrentUI");
            if (currentMenu == "ReviveMenu")
                CancelRevive();
        }
    }

    public static void OpenReviveMenu()
    {
        Time.timeScale = 0f;
        PlayerPrefs.SetString("CurrentUI", "ReviveMenu");
        ReviveManagerInstance.reviveCanvas.SetActive(true);
        ReviveManagerInstance.proposalCoroutine = ReviveManagerInstance.StartCoroutine(ReviveManagerInstance.ProposalTimer());
        MenuManager.HidePauseButton(); 
    }

    public void CancelRevive()
    {
        Time.timeScale = 1f;
        reviveCanvas.SetActive(false);
        PlayerPrefs.SetString("CurrentUI", "None");
        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt("Deaths_" + sceneName, PlayerPrefs.GetInt("Deaths_" + sceneName) + 1);
        SceneManager.LoadScene(sceneName);
    }

    public void Revive()
    {
        StopCoroutine(proposalCoroutine);
        EnemyMoviment[] enemys = FindObjectsOfType<EnemyMoviment>();
        foreach (EnemyMoviment enemy in enemys)
        {
            if (enemy.target == "Capturar" || enemy.target == "Player")
                enemy.Confuse();
        }
        reviveCanvas.SetActive(false);
        AdManager.OpenAd();
    }

    IEnumerator ProposalTimer()
    {
        float elapsedTime = 0f;
        timerSlider.value = 0f;

        while (elapsedTime < proposalDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            timerSlider.value = Mathf.Clamp01(elapsedTime / proposalDuration);
            yield return new WaitForEndOfFrame();
        }
        
        if (PlayerPrefs.GetString("CurrentUI") == "ReviveMenu")
            CancelRevive();
    }
}
