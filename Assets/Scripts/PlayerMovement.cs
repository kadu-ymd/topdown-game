using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSrcPoint;

    private Animator animator;
    public static Rigidbody2D rb;
    public float speed = 5f;

    private AudioSource playerAudioSource;
    public AudioClip walkingSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        playerAudioSource = GetComponent<AudioSource>();
        playerAudioSource.clip = walkingSound;
        playerAudioSource.loop = true;

        // Para o personagem começar de frente
        animator.SetFloat("LastHorizontal", 0f);
        animator.SetFloat("LastVertical", -1f);
        animator.SetBool("IsHit", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetString("CurrentUI") == "None")
        {
            if (!animator.GetBool("IsShooting"))
            {

                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");

                // Regra de prioridade para andar em somente uma direção
                if (vertical != 0) 
                {
                    horizontal = 0;
                } 
                else if (horizontal != 0) 
                {
                    vertical = 0;
                }

                animator.SetFloat("Horizontal", horizontal);
                animator.SetFloat("Vertical", vertical);
                bool isWalking = horizontal != 0 || vertical != 0;
                animator.SetBool("IsWalking", isWalking);

                if (isWalking && !playerAudioSource.isPlaying)
                {
                    playerAudioSource.Play();
                }
                else if (!isWalking && playerAudioSource.isPlaying)
                {
                    playerAudioSource.Stop();
                }

                if (horizontal != 0 || vertical != 0)
                {
                    animator.SetFloat("LastHorizontal", horizontal);
                    animator.SetFloat("LastVertical", vertical);
                }     
            }

            if (PlayerPrefs.GetInt("Gun") == 1) 
            {
                if (Input.GetKeyDown(KeyCode.Space) && !animator.GetBool("IsShooting"))
                {
                    animator.SetBool("IsShooting", true);
                    StartCoroutine(DelayedShoot(0.5f)); // Espera 0:50s da animação de tiro
                } 
                else if (Input.GetKeyUp(KeyCode.Space))
                {
                    animator.SetBool("IsShooting", false);
                }
            }
        }
        else
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            animator.SetBool("IsWalking", false);
            if (playerAudioSource.isPlaying)
            {
                playerAudioSource.Stop();
            }
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSrcPoint.position, Quaternion.identity);
        
        // Direção do tiro baseada na última direção do player
        Vector2 shootDirection = new Vector2(
            animator.GetFloat("LastHorizontal"),
            animator.GetFloat("LastVertical")
        );

        BulletController controller = bullet.GetComponent<BulletController>();
        controller.direction = shootDirection;
    }

    void FixedUpdate() 
    {
        if (!animator.GetBool("IsShooting"))
        {
            Vector2 movement = new Vector2(
                animator.GetFloat("Horizontal"),
                animator.GetFloat("Vertical")
            );

            rb.linearVelocity = movement * speed;
        } else {
            rb.linearVelocity = Vector2.zero; // Para o personagem enquanto atira
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    System.Collections.IEnumerator DelayedShoot(float delay)
    {
        yield return new WaitForSeconds(delay);
        Shoot();
        animator.SetBool("IsShooting", false);
    }
}
