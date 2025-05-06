using UnityEngine;

public class GuardMovement : MonoBehaviour
{
    private float atention_level;
    private bool stalk;
    public float speed;
    private Rigidbody2D rb;
    private Animator animator;
    private FieldOfView fieldOfView;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        fieldOfView = GetComponent<FieldOfView>();
    }

    void FixedUpdate()
    {
        animator.SetBool("Right", false);
        animator.SetBool("Left", false);
        animator.SetBool("Up", false);
        animator.SetBool("Down", false);

        if (stalk) 
        {
            animator.SetBool("IsWalking", true);

            Vector2 movement = (PlayerMovement.rb.position - rb.position).normalized;
            float Vx = movement.x;
            float Vy = movement.y;

            if (Mathf.Abs(Vx) > Mathf.Abs(Vy)) 
            {
                if (Vx > 0)
                {
                    animator.SetBool("Right", true);
                    fieldOfView.angleRotation = 90f;
                }
                else
                {
                    animator.SetBool("Left", true);
                    fieldOfView.angleRotation = -90f;
                }
            }
            else
            {
                if (Vy > 0)
                {
                    animator.SetBool("Up", true);
                    fieldOfView.angleRotation = 180f;
                }
                else
                {
                    animator.SetBool("Down", true);
                    fieldOfView.angleRotation = 0f;
                }
            }

            rb.MovePosition(rb.position + movement*Time.fixedDeltaTime*speed);
        }
        else
        {
            animator.SetBool("IsWalking", false);
            fieldOfView.angleRotation = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FieldOfView.canSeePlayer && atention_level < 2) 
        {
            atention_level += Time.deltaTime;
        }
        else if (!FieldOfView.canSeePlayer && atention_level > 0)
        {
            atention_level -= Time.deltaTime;
        }

        if (atention_level >= 2) 
        {
            stalk = true;
        }
        if (atention_level <= 0)
        {
            stalk = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player foi pego!");
        }
    }
}