using UnityEngine;
using System.Collections;

public class GhostSpanwer : MonoBehaviour
{

    private GameObject ghost;
    private SpriteRenderer ghostSpriteRenderer;
    private Color ghostOriginalColor;
    private Color ghostInitialColor;
    public GameObject ghostPrefab;
    public float respawnTime = 5f;
    public float fadeDuration = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghostSpriteRenderer = ghost.GetComponent<SpriteRenderer>();
        ghostOriginalColor = ghostSpriteRenderer.color;
        ghostInitialColor = ghostOriginalColor;
        ghostInitialColor.a = 0f; // Inicialmente invisível
        StartCoroutine(FadeIn());
        StartCoroutine(Reespawn());
    }

    public void SpawnGhost() {
        ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
        ghostSpriteRenderer = ghost.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn());
    }

    IEnumerator Reespawn() {
        while (ghost != null) {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(respawnTime);
        SpawnGhost();
        StartCoroutine(Reespawn());
    }
    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = ghostInitialColor;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, ghostOriginalColor.a, elapsedTime / fadeDuration);
            ghostSpriteRenderer.color = color;
            yield return new WaitForEndOfFrame();
        }
    }
}
