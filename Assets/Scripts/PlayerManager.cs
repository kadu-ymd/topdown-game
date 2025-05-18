using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PlayerManager : MonoBehaviour {
    private SpriteRenderer EkeySpriteRenderer;
    private GameObject interactableCloser;
    private Camera mainCamera;
    private Vector3 lastPlayerPosition;
    private Volume globalVolume;
    private ColorAdjustments colorAdjust;
    private float originalExposure;
    private bool originalExposureActive;
    private Animator animator;

    void Start() {
        EkeySpriteRenderer = transform.Find("E key").GetComponent<SpriteRenderer>();
        EkeySpriteRenderer.enabled = false;
        animator = GetComponent<Animator>();

        mainCamera = Camera.main;
        globalVolume  = GameObject.Find("Global Volume").GetComponent<Volume>();
        globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjust);
        originalExposure = colorAdjust.postExposure.value;
        originalExposureActive = colorAdjust.active;
    }
    
    void Update() {
        if (interactableCloser != null && Input.GetKeyDown(KeyCode.E)) {
            if (PlayerPrefs.GetString("CurrentUI") == "None") {
                animator.SetBool("IsHit", true);
                StartCoroutine(ResetHitFlag(0.25f));
                interactableCloser.GetComponent<InteractableManager>().Interact();
                EkeySpriteRenderer.enabled = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Interactable"))
        {
            EkeySpriteRenderer.enabled = true;
            interactableCloser = collider.gameObject;

        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (collider.CompareTag("Interactable") && interactableCloser == collider.gameObject) {
            EkeySpriteRenderer.enabled = false;
            interactableCloser = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            lastPlayerPosition = transform.position;
            StartCoroutine(HandlePlayerDefeat());
        }
    }
    
    IEnumerator ResetHitFlag(float delay) {
        yield return new WaitForSeconds(delay);
        animator.SetBool("IsHit", false);
    }

    private IEnumerator HandlePlayerDefeat()
    {
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 0f;

        lastPlayerPosition.z = mainCamera.transform.position.z;

        colorAdjust.active = true;
        StartCoroutine(FadeExposure(-10f, 1f)); // fade para escuro em 1 segundo

        // Tremor da câmera
        float shakeDuration = 0.5f;
        float shakeMagnitude = 0.2f;
        float shakeFreq = 20f;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float offsetX = Mathf.Sin(Time.unscaledTime * shakeFreq) * shakeMagnitude;
            float offsetY = Mathf.Cos(Time.unscaledTime * shakeFreq) * shakeMagnitude;

            mainCamera.transform.position = new Vector3(
                lastPlayerPosition.x + offsetX,
                lastPlayerPosition.y + offsetY,
                mainCamera.transform.position.z
            );

            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.5f);

        Time.timeScale = 1f;
        string sceneName = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetInt("Deaths_" + sceneName, PlayerPrefs.GetInt("Deaths_" + sceneName) + 1);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeExposure(float targetExposure, float duration) {
        float startExposure = colorAdjust.postExposure.value;
        float elapsed = 0f;

        while (elapsed < duration) {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            colorAdjust.postExposure.value = Mathf.Lerp(startExposure, targetExposure, t);
            yield return null;
        }

        colorAdjust.postExposure.value = targetExposure;
    }
}