using UnityEngine;
using System.Collections;

public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)]
    public float angle;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    public float angleRotation = 0f;

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

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Transform target = other.transform;

            Vector2 directionToTarget = (target.position - transform.position).normalized;
            Vector2 adjustedDirection = RotateVector(Vector2.down, angleRotation);

            // Verifica se o jogador está dentro do campo de visão
            if (Vector2.Angle(adjustedDirection, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                // Verifica se há um obstáculo entre o inimigo e o jogador
                canSeePlayer = !Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask);
            }
            else
                canSeePlayer = false;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player" && canSeePlayer)
            canSeePlayer = false;
    }
}
