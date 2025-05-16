using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using TMPro;

public class ThoughtManager : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static ThoughtManager thoughtManagerInstance;
    private static TMP_Text thoughtTextTMP;
    private static TMP_Text NameTextTMP;
    private static RectTransform thoughtTextRectTransform;
    private static AudioSource audioSource;
    public AudioClip thoughtSound;
    public ScrollRect scrollRect;
    public static bool thinking = false;
    private bool skip = false;
    private float delay = 0.05f;
    public float thoughtDuration = 5f;

    void Awake() {
        if (thoughtManagerInstance == null) thoughtManagerInstance = this;
    }

    void Start() {
        foreach (TMP_Text TMPtext in GetComponentsInChildren<TMP_Text>()) {
            if (TMPtext.gameObject.name == "ThoughtText") {
                thoughtTextTMP = TMPtext;
                thoughtTextRectTransform = TMPtext.GetComponent<RectTransform>();
            } else if (TMPtext.gameObject.name == "NameText") {
                NameTextTMP = TMPtext;
            }
        }

        if (thoughtTextTMP == null)  Debug.LogError("Thought Text TMP not found. Please ensure it is named 'ThoughtText' in the scene.");
        if (NameTextTMP == null)  Debug.LogError("Thought Text TMP not found. Please ensure it is named 'NameText' in the scene.");
        gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        if (!PlayerPrefs.HasKey("Name")) PlayerPrefs.SetString("Name", "????");
    }

    void Update() {
        if (Input.anyKeyDown) {
            if (PlayerPrefs.GetString("CurrentUI") == "Thought") {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E)) {
                    if (!skip) skip = true;
                    else HideThought();
                } else {
                    StartCoroutine(StopThought());
                }
            }
        }
    }

    public static void ShowThought(string thoughtText) {
        if (!thinking) {
            thinking = true;
            thoughtManagerInstance.gameObject.SetActive(true);
            thoughtTextTMP.text = "";
            PlayerPrefs.SetString("CurrentUI", "Thought");
            audioSource.PlayOneShot(thoughtManagerInstance.thoughtSound);
            thoughtManagerInstance.StartCoroutine(thoughtManagerInstance.RevealText(thoughtText));
        }
    }

    public void HideThought() {
        gameObject.SetActive(false);
        PlayerPrefs.SetString("CurrentUI", "None");
        skip = false;
        thinking = false;
    }

    private IEnumerator StopThought() {
        yield return new WaitForSeconds(thoughtDuration);
        HideThought();
    }

    private IEnumerator RevealText(string text) {
        foreach (char letter in text) {
            if (skip) {
                thoughtTextTMP.text = text;
                UpdateTextSize();
                yield break;
            }
            thoughtTextTMP.text += letter;
            UpdateTextSize();
            yield return new WaitForSeconds(delay);
        }
        skip = true;
    }
    private void UpdateTextSize() {
        Canvas.ForceUpdateCanvases(); 
        float tmpHeight = thoughtTextTMP.preferredHeight;
        float maxHeight = scrollRect.viewport.rect.height;

        thoughtTextRectTransform.sizeDelta = new Vector2(thoughtTextRectTransform.sizeDelta.x, tmpHeight);
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, tmpHeight);

        if (tmpHeight > maxHeight) StartCoroutine(SmoothScroll());
    }
    private IEnumerator SmoothScroll() {
        yield return new WaitForEndOfFrame(); // Espera um frame para atualizar o layout

        // Ajusta a posição do scroll gradualmente sem travar
        thoughtManagerInstance.scrollRect.verticalNormalizedPosition = Mathf.Clamp(
            thoughtManagerInstance.scrollRect.verticalNormalizedPosition - 0.05f, 0f, 1f
        );
    }
}

