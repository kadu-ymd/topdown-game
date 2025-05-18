using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyDeathEffect : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip heartbeatClip;
    public AudioClip tinnitusClip;

    private Camera mainCamera;
    private ColorAdjustments colorAdjust;
    private float originalExposure;
    private bool originalExposureActive;
    private Volume globalVolume;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainCamera = Camera.main;
        globalVolume  = GameObject.Find("Global Volume").GetComponent<Volume>();
        globalVolume.profile.TryGet<ColorAdjustments>(out colorAdjust);
        originalExposure = colorAdjust.postExposure.value;
        originalExposureActive = colorAdjust.active;

        StartCoroutine(OnEnemyDeath());
    }

    IEnumerator OnEnemyDeath()
    {
        audioSource.clip = heartbeatClip;
        audioSource.Play();
        audioSource.PlayOneShot(tinnitusClip);

        Debug.Log("Dead");
        yield return StartCoroutine(FadeExposure(-10f, 0.25f));
        yield return StartCoroutine(FadeExposure(originalExposure, 0.2f));
        yield return StartCoroutine(CameraShake(0.03f, 0.1f, 10f));
    }

    private IEnumerator CameraShake(float duration, float magnitude, float frequency)
    {
        float elapsed = 0f;
        Vector3 originalPos = mainCamera.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / duration;
            float damping = 1f - Mathf.Clamp01(progress);
            Vector2 shakeOffset = Random.insideUnitCircle * magnitude * damping;

            mainCamera.transform.position = new Vector3(
                originalPos.x + shakeOffset.x,
                originalPos.y + shakeOffset.y,
                originalPos.z
            );

            yield return new WaitForSecondsRealtime(1f / frequency);
        }

        mainCamera.transform.position = originalPos;
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