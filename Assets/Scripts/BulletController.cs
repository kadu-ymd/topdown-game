using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float velocity = 5f;
    public Vector2 direction;
    private Rigidbody2D rb;
    public AudioClip shootClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (Time.timeSinceLevelLoad > 1f)
        {
            AudioSource.PlayClipAtPoint(shootClip, transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * velocity;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Destrói o inimigo
            Destroy(gameObject); // Destrói a bala
        }
        else if (!other.CompareTag("Player"))
        {
            Destroy(gameObject); // Destrói a bala ao atingir outros objetos
        }
    }

}
