using UnityEngine;
using System.Collections;

public class GhostSpanwer : MonoBehaviour
{
    private GameObject originalGhost;
    private GameObject ghost;
    private SpriteRenderer ghostSpriteRenderer;
    private Color ghostOriginalColor;
    private Color ghostInitialColor;
    public float respawnTime = 5f;
    public float fadeDuration = 2f;
    public bool peaceful = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        originalGhost = GetComponentInChildren<EnemyMoviment>().gameObject;
        originalGhost.SetActive(false);
        SpriteRenderer originalGhostSpriteRenderer = originalGhost.GetComponent<SpriteRenderer>();
        ghostOriginalColor = originalGhostSpriteRenderer.color;
        ghostInitialColor = ghostOriginalColor;
        ghostInitialColor.a = 0f; // Inicialmente invisível
        if (peaceful)
        {
            GhostMoviment ghostMoviment = originalGhost.GetComponent<GhostMoviment>();
            if (ghostMoviment != null)
                ghostMoviment.peaceful = true;
            else
            {
                GhostRoute ghostRoute = originalGhost.GetComponent<GhostRoute>();
                if (ghostRoute != null)
                    ghostRoute.peaceful = true;
                else
                    Debug.LogWarning("O objeto originalGhost não possui um componente GhostMoviment ou GhostRoute.");
            }
        }
        SpawnGhost();
        StartCoroutine(Reespawn());
    }

    public void SpawnGhost() {
        ghost = Instantiate(originalGhost, transform.position, transform.rotation);
        ghost.SetActive(true);
        ghostSpriteRenderer = ghost.GetComponent<SpriteRenderer>();
        ghostSpriteRenderer.color = ghostInitialColor;
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
