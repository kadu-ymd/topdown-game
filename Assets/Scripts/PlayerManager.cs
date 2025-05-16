using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PlayerManager : MonoBehaviour {
    public SpriteRenderer EkeySpriteRenderer;
    
    private Camera mainCamera;
    private Vector3 lastPlayerPosition;

    private Volume globalVolume;
    private ColorAdjustments colorAdjust;
    private float originalExposure;
    private bool originalExposureActive;

    void Start() {
        this.EkeySpriteRenderer = transform.Find("E key").GetComponent<SpriteRenderer>();
        this.EkeySpriteRenderer.enabled = false;

        mainCamera = Camera.main;
        globalVolume  = GameObject.Find("Global Volume").GetComponent<Volume>();
        globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjust);
        originalExposure = colorAdjust.postExposure.value;
        originalExposureActive = colorAdjust.active;
    }

    private void OnTriggerStay2D(Collider2D gameObject) {
        if (gameObject.CompareTag("Interactable")) {
            this.EkeySpriteRenderer.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D gameObject) {
        if (gameObject.CompareTag("Interactable")) {
            this.EkeySpriteRenderer.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D gameObject) {
        if (gameObject.gameObject.CompareTag("Enemy")) {
            lastPlayerPosition = transform.position;
            StartCoroutine(HandlePlayerDefeat());
        }
    }

    private IEnumerator HandlePlayerDefeat() {
        Time.timeScale = 0f;

        lastPlayerPosition.z = mainCamera.transform.position.z;

        colorAdjust.active = true;
        StartCoroutine(FadeExposure(-10f, 1f)); // fade para escuro em 1 segundo

        // Tremor da câmera
        float shakeDuration = 0.5f;
        float shakeMagnitude = 0.2f;
        float shakeFreq = 20f;
        float elapsed = 0f;

        while (elapsed < shakeDuration) {
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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



    void Update() {
    }
}