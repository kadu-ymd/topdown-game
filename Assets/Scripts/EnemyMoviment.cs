using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyMoviment : MonoBehaviour
{
    protected AudioSource audioSource;
    protected static AudioSource currentPlayingAudio = null;
    public float maxAttention = 2f;
    protected NavMeshAgent agent;

    [HideInInspector] public float atention_level = 0f;
    protected bool stalk;
    protected Vector2 initialPosition;
    protected string target = "Nenhum";

    protected Rigidbody2D rb;
    protected Animator animator;
    protected FieldOfView fieldOfView;

    public GameObject deadBodyPrefab;
    public GameObject memoryPrefab;
    public string memoryName;

    private Coroutine sleepCoroutine;
    private bool sleepy = false;
    public bool sleeping = false;
    public float sleepingTime = 0f;
    public float awakedTime = 0f;

    protected virtual void Start()
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

        if (string.IsNullOrEmpty(memoryName))
        {
            if (!PlayerPrefs.HasKey(memoryName))
                PlayerPrefs.SetInt(memoryName, 0);

            if (memoryPrefab == null && !string.IsNullOrEmpty(memoryName))
                Debug.LogError("Memory prefab não atribuído no inspetor.");
        }

        animator.SetFloat("Vx", 0f);
        animator.SetFloat("Vy", -1f);
    }

    protected virtual void FixedUpdate() // Atualização do animator e target
    {
        // State Machine
        if (target == "Hit")
            animator.SetTrigger("Hit");

        else if (target == "Player")
        {
            sleepy = false;
            MoveTo(PlayerMovement.rb.position);
        }
        else if (target == "Inicio")
        {
            sleepy = false;
            if (stalk)
                StartStalk();
            else
            {
                MoveTo(initialPosition);

                if (Mathf.Abs(Vector2.Distance(initialPosition, rb.position)) < 0.1f)
                {
                    target = "Nenhum";
                    animator.SetBool("IsWalking", false);
                    animator.SetFloat("Vx", 0f);
                    animator.SetFloat("Vy", -1f);
                }
            }
        }
        else if (target == "Nenhum")
        {
            fieldOfView.angleRotation = 0f;
            if (sleepCoroutine == null && sleepingTime > 0 && awakedTime > 0)
                sleepCoroutine = StartCoroutine(Sleep());
        }
    }

    protected void Update() // atualização do stalk
    {
        if (!sleeping)
        {

            if (fieldOfView.canSeePlayer && atention_level < maxAttention)
            {
                sleepy = false;
                atention_level += Time.deltaTime;
            }
            else if (!fieldOfView.canSeePlayer && atention_level > 0)
            {
                if (stalk)
                    atention_level -= Time.deltaTime/4;
                else
                    atention_level -= Time.deltaTime;
            }

            if (!stalk && atention_level >= maxAttention) // muda para o estado de perseguicão
            {
                StartStalk();
            }
            else if (stalk && atention_level <= 0)
            {
                stalk = false;
                target = "Inicio";
            }

            playAudio();
        }
    }

    protected void StartStalk() {
        if (!animator.GetBool("IsWalking")) // inicia a animação de andar
        {
            animator.SetBool("IsWalking", true);
            animator.SetTrigger("Down");

        }
        stalk = true;
        target = "Player";
    }

    protected void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            target = "Hit";
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            string sceneName = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetInt("kills_" + sceneName, PlayerPrefs.GetInt("kills_" + sceneName) + 1);
            SwaptoDead();
        }

    }

    protected void MoveTo(Vector2 destiny)
    {
        Vector2 movement = (destiny - rb.position).normalized;
        agent.SetDestination(destiny);
        SetFlagsAnimation(movement.x, movement.y);
    }

    protected void SetFlagsAnimation(float Vx, float Vy)
    {
        animator.SetFloat("Vx", Vx);
        animator.SetFloat("Vy", Vy);
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
            {
                if (Mathf.Abs(fieldOfView.angleRotation) > 0.1f)
                {
                    animator.SetTrigger("Down");
                    fieldOfView.angleRotation = 0f;
                }
            }
        }
    }

    protected void SwaptoDead()
    {
        Vector3 currentPosition = transform.position;

        // Instancia o novo inimigo na mesma posição e rotação
        Instantiate(deadBodyPrefab, currentPosition, deadBodyPrefab.transform.rotation);

        // Instancia a memória na mesma posição e rotação
        if (!string.IsNullOrEmpty(memoryName) && PlayerPrefs.GetInt(memoryName) == 0)
        {
            Instantiate(memoryPrefab, currentPosition, memoryPrefab.transform.rotation);
            PlayerPrefs.SetInt(memoryName, 1);
        }

        // Destroi o inimigo atual
        Destroy(gameObject);
    }

    void playAudio()
    {
        float volumePercent = Mathf.Clamp01(atention_level / maxAttention);
        audioSource.volume = volumePercent;

        if (volumePercent > 0f)
        {
            if (!audioSource.isPlaying && (currentPlayingAudio == null || !currentPlayingAudio.isPlaying))
            {
                audioSource.Play();
                currentPlayingAudio = audioSource;
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                if (currentPlayingAudio == audioSource)
                {
                    currentPlayingAudio = null;
                }
            }
        }
    }

    IEnumerator Sleep()
    {
        sleepy = true;
        yield return new WaitForSeconds(awakedTime);
        if (target == "Nenhum" && sleepy)
        {
            target = "Dormir";
            sleeping = true;
            yield return new WaitForSeconds(sleepingTime);
            sleeping = false;
            sleepy = false;
            target = "Nenhum";
        }
        sleepCoroutine = null;
    }
}