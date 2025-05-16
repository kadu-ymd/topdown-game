using UnityEngine;
using UnityEngine.AI;

public class EnemyMoviment : MonoBehaviour
{
    protected AudioSource audioSource;
    public float maxAttention = 2f;
    protected NavMeshAgent agent;

    public float atention_level;
    protected bool stalk;
    protected Vector2 initialPosition;
    protected string target;

    public float speed;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected FieldOfView fieldOfView;

    public GameObject DeadBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        // Navigator
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        fieldOfView = GetComponentInChildren<FieldOfView>();
        audioSource = GetComponent<AudioSource>();
        initialPosition = rb.position;
    }

    public virtual void FixedUpdate() // Atualização do animator e target
    {
        // State Machine
        if (target == "Hit")
        {
            animator.SetTrigger("Hit");
        }
        else if (target == "Player")
        {
            moveToPlayer();
            if (!stalk)
            {
                target = "Inicio";
            }
        }
        else if (target == "Inicio")
        {
            moveToStart();
            if (Mathf.Abs(Vector2.Distance(initialPosition, rb.position)) < 0.1f)
            {
                target = "Nenhum";
                animator.SetBool("IsWalking", false);
            }
        }
        else if (target == "Nenhum")
        {
            fieldOfView.angleRotation = 0f;
        }
    }

    // Update is called once per frame
    void Update() // atualização do stalk
    {
        if (fieldOfView.canSeePlayer && atention_level < 2) 
        {
            atention_level += Time.deltaTime;
        }
        else if (!fieldOfView.canSeePlayer && atention_level > 0)
        {
            atention_level -= Time.deltaTime;
        }

        if (atention_level >= 2) 
        {
            if (!animator.GetBool("IsWalking"))
            {
                animator.SetBool("IsWalking", true);
                animator.SetTrigger("Down");
            }
            stalk = true;
            target = "Player";
        }
        if (atention_level <= 0)
        {
            stalk = false;
        }

        playAudio();
    }

    protected void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            target = "Hit";
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Bullet");
            SwaptoDead();
        }
    }

    protected void moveToPlayer()
    {
        Vector2 movement = (PlayerMovement.rb.position - rb.position).normalized;
        agent.SetDestination(PlayerMovement.rb.position);

        float Vx = movement.x;
        float Vy = movement.y;
        SetFlagsAnimation(Vx, Vy);
    }

    protected void moveToStart()
    {
        Vector2 movement = (initialPosition - rb.position).normalized;
        agent.SetDestination(initialPosition);

        float Vx = movement.x;
        float Vy = movement.y;
        SetFlagsAnimation(Vx, Vy);
    }

    protected void SetFlagsAnimation(float Vx, float Vy)
    {
        if (Mathf.Abs(Vx) > Mathf.Abs(Vy)) 
        {
            if (Vx > 0)
            {
                if (Mathf.Abs(fieldOfView.angleRotation - 90f) > 0.1f)
                {
                    animator.SetTrigger("Right");
                    fieldOfView.angleRotation = 90f;
                }
            }
            else
            {
                if (Mathf.Abs(fieldOfView.angleRotation + 90f) > 0.1f)
                {
                    animator.SetTrigger("Left");
                    fieldOfView.angleRotation = -90f;
                }
            }
        }
        else
        {
            if (Vy > 0)
            {
                if (Mathf.Abs(fieldOfView.angleRotation - 180f) > 0.1f)
                {
                    animator.SetTrigger("Up");
                    fieldOfView.angleRotation = 180f;
                }
            }
            else
                if (Mathf.Abs(fieldOfView.angleRotation) > 0.1f)
                {
                    animator.SetTrigger("Down");
                    fieldOfView.angleRotation = 0f;
                }
        }
    }

    protected void SwaptoDead()
    {
        Vector3 currentPosition = transform.position;

        // Instancia o novo inimigo na mesma posição e rotação
        Instantiate(DeadBody, currentPosition, DeadBody.transform.rotation);

        // Destroi o inimigo atual
        Destroy(gameObject);
    }

    void playAudio() {     
        float volumePercent = Mathf.Clamp01(atention_level / maxAttention);
        audioSource.volume = volumePercent;

        // Controle de play/stop automático
        if (volumePercent > 0f && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (volumePercent <= 0f && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}