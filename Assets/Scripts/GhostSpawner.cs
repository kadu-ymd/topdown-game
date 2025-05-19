using UnityEngine;
using System.Collections;

public class GhostSpawner : MonoBehaviour
{
    private GameObject originalGhost;
    private GameObject ghost;
    private SpriteRenderer ghostSpriteRenderer;
    private Color ghostOriginalColor;
    private Color ghostInitialColor;
    public float respawnTime = 5f;
    public float fadeDuration = 2f;
    public bool peaceful = false;
    public bool canSpawn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalGhost = GetComponentInChildren<EnemyMoviment>().gameObject;
        SpriteRenderer originalGhostSpriteRenderer = originalGhost.GetComponent<SpriteRenderer>();
        ghostOriginalColor = originalGhostSpriteRenderer.color;
        ghostInitialColor = ghostOriginalColor;
        ghostInitialColor.a = 0f; // Inicialmente invisível
        originalGhost.SetActive(false);
        StartCoroutine(Reespawn());
    }

    public void SpawnGhost() {
        ghost = Instantiate(originalGhost, transform.position, transform.rotation);
        ghostSpriteRenderer = ghost.GetComponent<SpriteRenderer>();
        ghostSpriteRenderer.color = ghostInitialColor;
        if (peaceful)
            SetPeaceful();
        ghost.SetActive(true);
        StartCoroutine(FadeIn());
    }

    public void SetPeaceful() {
        this.peaceful = peaceful;
        if (ghost != null) {
            GhostMoviment ghostMoviment = ghost.GetComponent<GhostMoviment>();
            GhostRoute ghostRoute = ghost.GetComponent<GhostRoute>();
            if (ghostMoviment != null)
                ghostMoviment.peaceful = true;
            else if (ghostRoute != null)
                ghostRoute.peaceful = true;
            else
                Debug.LogWarning("O objeto ghost não possui um componente GhostMoviment ou GhostRoute.");
        
        }
    }

    IEnumerator Reespawn() {
        while (ghost != null || !canSpawn) {
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
