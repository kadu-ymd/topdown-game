using UnityEngine;

public class ItemDisplayManager : MonoBehaviour {
    public GameObject pauseMenuPrefab;
    public string defaultExitDisplayText;
    private string exitDisplayText;

    void Awake() {
        gameObject.SetActive(false);
    }

    public void EnterDisplay(string exitDisplayText = null) {
        pauseMenuPrefab.SetActive(false);
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        if (!string.IsNullOrEmpty(exitDisplayText)) {
            this.exitDisplayText = exitDisplayText;
        }
    }

    public void ExitDisplay() {
        pauseMenuPrefab.SetActive(true);
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        if (!string.IsNullOrEmpty(exitDisplayText)) {
            ThoughtManager.ShowThought(exitDisplayText);
        } else if (!string.IsNullOrEmpty(defaultExitDisplayText)) {
            ThoughtManager.ShowThought(defaultExitDisplayText);
        }
    }
}
