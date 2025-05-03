using UnityEngine;

public class GuardMovement : MonoBehaviour
{
    private float atention_level;
    private bool stalk;
    public float speed;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (stalk) 
        {
            Vector2 movement = PlayerMovement.rb.position - rb.position;
            rb.MovePosition(rb.position + movement*Time.fixedDeltaTime*speed);
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
}