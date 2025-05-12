using UnityEngine;

public class ItemDisplayManager : MonoBehaviour {
    public string defaultExitDisplayText;
    private string exitDisplayText;

    void Awake() {    
        gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerPrefs.GetString("CurrentUI") == "ItemDisplay") {
            ExitDisplay();
        }
    }

    public void EnterDisplay(string exitDisplayText = null) {
        MenuManager.HidePauseButton();
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        PlayerPrefs.SetString("CurrentUI", "ItemDisplay");
        if (!string.IsNullOrEmpty(exitDisplayText)) {
            this.exitDisplayText = exitDisplayText;
        }
    }

    public void ExitDisplay() {
        MenuManager.ShowPauseButton();
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        PlayerPrefs.SetString("CurrentUI", "None");
        if (!string.IsNullOrEmpty(exitDisplayText)) {
            ThoughtManager.ShowThought(exitDisplayText);
        } else if (!string.IsNullOrEmpty(defaultExitDisplayText)) {
            ThoughtManager.ShowThought(defaultExitDisplayText);
        }
    }
}
