using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GhostMoviment : EnemyMoviment
{
    private Collider2D spriteCollider;
    private SpriteRenderer spriteRenderer;
    public bool peaceful;
    public float fadeDuration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        spriteCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (peaceful)
            MakePeace();
    }

    void FixedUpdate()
    {
        if (!peaceful)
            base.FixedUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (peaceful) {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);
            Vector2 playerDirection = (PlayerMovement.rb.position - position).normalized;
            SetFlagsAnimation(playerDirection.x, playerDirection.y);
        }
        else
            base.Update();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!peaceful && collision.gameObject.CompareTag("Player"))
            target = "Hit";
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Bullet"))
        {
            MakePeace();
            StartCoroutine(FadeOut());
        }
    }

    void MakePeace()
    {
        peaceful = true;
        atention_level = 0f;
        animator.SetBool("IsWalking", false);
        spriteCollider.enabled = false;
        rb.simulated = false;
        gameObject.tag = "Untagged";
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;
        Color color = originalColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = color;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }

}