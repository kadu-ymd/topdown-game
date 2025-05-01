using UnityEngine;

public class PatientMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public float speed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Para o personagem começar de frente
        animator.SetFloat("LastHorizontal", 0f);
        animator.SetFloat("LastVertical", -1f);
    }

    // Update is called once per frame
    void Update()
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
        Debug.Log($"IsWalking: {animator.GetBool("IsWalking")}, LastVertical: {animator.GetFloat("LastVertical")}, LastHorizontal: {animator.GetFloat("LastHorizontal")}");
     
    }

    void FixedUpdate() 
    {
        Vector2 movement = new Vector2(
            animator.GetFloat("Horizontal"),
            animator.GetFloat("Vertical")
        );

        rb.linearVelocity = movement * speed;
    }
}
