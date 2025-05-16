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
        animator.SetBool("Right", false);
        animator.SetBool("Left", false);
        animator.SetBool("Up", false);
        animator.SetBool("Down", false);

        // State Machine
        if (target == "Player")
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
            }
        }
        else if (target == "Nenhum")
        {
            animator.SetBool("IsWalking", false);
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
            stalk = true;
            target = "Player";
            animator.SetBool("IsWalking", true);
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