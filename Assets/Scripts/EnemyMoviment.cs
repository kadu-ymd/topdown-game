using UnityEngine;

public class EnemyMoviment : MonoBehaviour
{
    public float atention_level;
    private bool stalk;
    public float speed;
    private Rigidbody2D rb;
    private Animator animator;
    private FieldOfView fieldOfView;
    private Vector2 initialPosition;
    private string target;

    string SetFlagsAnimation(string target)
    {
        Vector2 movement = new Vector2(0, 0);
        if (target == "Player")
        {
            movement = (PlayerMovement.rb.position - rb.position).normalized;
        }
        else if (target == "Inicio")
        {
            movement = (initialPosition - rb.position).normalized;
        }
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

        if (target == "Inicio" && Mathf.Abs(Vector2.Distance(initialPosition, rb.position)) < 0.1f)
        {
            target = "Nenhum";
        }
        return target;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        fieldOfView = GetComponent<FieldOfView>();
        initialPosition = rb.position;
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
            target = "Player";
        }
        else if (!stalk && target == "Player")
        {
            target = "Inicio";
        }
        else if (target == "Nenhum")
        {
            animator.SetBool("IsWalking", false);
            fieldOfView.angleRotation = 0f;
        }
        target = SetFlagsAnimation(target);
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
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
}