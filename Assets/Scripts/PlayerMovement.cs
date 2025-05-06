using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    public static Rigidbody2D rb;
    private Transform tf;
    public float speed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();

        // Para o personagem começar de frente
        animator.SetFloat("LastHorizontal", 0f);
        animator.SetFloat("LastVertical", -1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.0)
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
                animator.SetBool("IsWalking", horizontal != 0 || vertical != 0);

                if (horizontal != 0 || vertical != 0)
                {
                    animator.SetFloat("LastHorizontal", horizontal);
                    animator.SetFloat("LastVertical", vertical);
                }     
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("IsShooting", true);
            } 
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                animator.SetBool("IsShooting", false);
            }
        }
        else
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            animator.SetBool("IsWalking", false);
        }
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

        tf.position = new Vector3(tf.position.x, tf.position.y, tf.position.y);
    }
}
