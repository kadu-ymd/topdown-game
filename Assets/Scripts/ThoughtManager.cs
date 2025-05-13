using UnityEngine;
using TMPro;

public class ThoughtManager : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static ThoughtManager thoughtManagerInstance;
    private static TMP_Text thoughtTextTMP;
    private static AudioSource audioSource;
    public AudioClip thoughtSound;

    void Awake() {
        if (thoughtManagerInstance == null) thoughtManagerInstance = this;
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
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerPrefs.GetString("CurrentUI") == "Thought") {
            HideThought();
        }
    }

    public static void ShowThought(string thoughtText) {
        thoughtManagerInstance.gameObject.SetActive(true);
        thoughtTextTMP.text = thoughtText;
        Time.timeScale = 0f;
        PlayerPrefs.SetString("CurrentUI", "Thought");
        audioSource.PlayOneShot(thoughtManagerInstance.thoughtSound);
    }

    public void HideThought() {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        PlayerPrefs.SetString("CurrentUI", "None");
    }
}
