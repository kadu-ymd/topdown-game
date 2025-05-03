using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)]
    public float angle;
    
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public static bool canSeePlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void OnTriggerStay2D(Collider2D other) {

        if (other.tag == "Player") {
            Transform target = other.transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle((Vector2)transform.position, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    Debug.Log("canSeePlayer");
                }
                else
                {
                    Debug.Log("Obstacle");
                    canSeePlayer = false;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {

        if (other.tag == "Player" && canSeePlayer) {
            canSeePlayer = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
