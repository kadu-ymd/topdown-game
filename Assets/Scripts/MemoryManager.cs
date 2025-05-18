using UnityEngine;
using System.Collections;

public class MemoryManager : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private Vector3 initialScale;
    private Vector3 targetScale;
    private bool goToPlayer = false;
    private float elapsedTime = 0f;
    public float followDistance = 5f;
    public float moveSpeed = 5f;
    public float rememberDuration = 2f;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player").transform;
        initialScale = transform.localScale;
        targetScale = initialScale * 2f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float hue = (elapsedTime / 3f) % 1f;
        spriteRenderer.color = Color.HSVToRGB(hue, 0.3f, 1f);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (goToPlayer)
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        else if (distanceToPlayer > followDistance || distanceToPlayer < 0.5f)
            goToPlayer = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Remember());
            goToPlayer = true;
        }
    }

    IEnumerator Remember()
    {
        float elapsedTime = 0f;

        while (elapsedTime < rememberDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / rememberDuration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = targetScale;
        BookManager.DisplayBookIntoPage(BookManager.UpdatedeBookPages(), "... ... Essa memória dolorosa... Ela não pode ser real, pode?");
        Destroy(gameObject);
    }
}
