using UnityEngine;
using TMPro;

public class ThoughtManager : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static ThoughtManager instance;
    private static TMP_Text thoughtTextTMP;

    void Awake() {
        if (instance == null) instance = this;
    }

    void Start() {
        foreach (TMP_Text TMPtext in GetComponentsInChildren<TMP_Text>()) {
            if (TMPtext.gameObject.name == "ThoughtText") {
                thoughtTextTMP = TMPtext;
                break;
            }
        }
        if (thoughtTextTMP == null)  Debug.LogError("Thought Text TMP not found. Please ensure it is named 'ThoughtText' in the scene.");
        gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            HideThought();
        }
    }

    public static void ShowThought(string thoughtText) {
        instance.gameObject.SetActive(true);
        thoughtTextTMP.text = thoughtText;
        Time.timeScale = 0f;
    }

    public void HideThought() {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
