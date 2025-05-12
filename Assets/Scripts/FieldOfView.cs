using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)]
    public float angle;
    
    public LayerMask obstructionMask;
    public static bool canSeePlayer;
    public float angleRotation;

    Vector2 RotateVector(Vector2 original, float angleDegrees)
    {
        float radians = angleDegrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(
            original.x * cos - original.y * sin,
            original.x * sin + original.y * cos
        );
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void OnTriggerStay2D(Collider2D other) {

        if (other.tag == "Player") {
            Transform target = other.transform;

            Vector2 directionToTarget = (target.position - transform.position).normalized;
            Vector2 adjustedDirection = RotateVector(Vector2.down, angleRotation);

            if (Vector2.Angle(adjustedDirection, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                }
                else
                {
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
