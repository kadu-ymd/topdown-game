using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using TMPro;

public class WifeManager : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private static WifeManager wifeManagerInstance;
    private static TMP_Text thoughtTextTMP;
    private static RectTransform thoughtTextRectTransform;
    private static AudioSource audioSource;
    private Touch touch;
    public AudioClip thoughtSound;
    public ScrollRect scrollRect;
    public static bool thinking = false;
    private bool skip = false;
    private float delay = 0.05f;

    void Awake() {
        if (wifeManagerInstance == null) wifeManagerInstance = this;
    }

    void Start() {
        foreach (TMP_Text TMPtext in GetComponentsInChildren<TMP_Text>()) {
            if (TMPtext.gameObject.name == "ThoughtText")
            {
                thoughtTextTMP = TMPtext;
                thoughtTextRectTransform = TMPtext.GetComponent<RectTransform>();
                break;
            } 
        }

        if (thoughtTextTMP == null)  Debug.LogError("Thought Text TMP not found. Please ensure it is named 'ThoughtText' in the scene.");
        gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && PlayerPrefs.GetString("CurrentUI") == "Thought")
            {
                if (!skip) skip = true;
                else HideThought();
            }
        }
    }

    public static void ShowThought(string thoughtText) {
        if (!thinking) {
            thinking = true;
            wifeManagerInstance.gameObject.SetActive(true);
            thoughtTextTMP.text = "";
            PlayerPrefs.SetString("CurrentUI", "WifeThought");
            audioSource.PlayOneShot(wifeManagerInstance.thoughtSound);
            wifeManagerInstance.StartCoroutine(wifeManagerInstance.RevealText(thoughtText));
        }
    }

    public static void HideThought() {
        wifeManagerInstance.gameObject.SetActive(false);
        PlayerPrefs.SetString("CurrentUI", "None");
        wifeManagerInstance.skip = false;
        thinking = false;
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
        wifeManagerInstance.scrollRect.verticalNormalizedPosition = Mathf.Clamp(
            wifeManagerInstance.scrollRect.verticalNormalizedPosition - 0.05f, 0f, 1f
        );
    }
}

