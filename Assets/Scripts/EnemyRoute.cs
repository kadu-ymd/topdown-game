using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyRoute : EnemyMoviment
{
    public List<Vector2> route = new List<Vector2>();
    private int patrolPosition;
    private bool going;
    public bool cyclicPatrol = false;
    protected override void Start()
    {
        base.Start();

        route.Insert(0, rb.position);

        target = "Patrol";
        going = true;

        animator.SetBool("IsWalking", true);
        animator.SetTrigger("Down");
    }

    protected override void FixedUpdate() // Atualiza do animator e target
    {
        // State Machine
        if (target == "Hit")
        {
            animator.SetTrigger("Hit");
        }
        else if (target == "Player")
        {
            MoveTo(PlayerMovement.rb.position);
            if (!stalk)
            {
                target = "Inicio";
            }
        }
        else if (target == "Inicio")
        {
            MoveTo(initialPosition);
            if (Mathf.Abs(Vector2.Distance(initialPosition, rb.position)) < 0.1f)
            {
                patrolPosition = 0;
                target = "Patrol";
            }
        }
        else if (target == "Patrol")
        {
            moveOnPatrol();
        }
    }

    void moveOnPatrol()
    {
        Vector2 destiny = route[patrolPosition];
        int nPoints = route.Count;

        Vector2 movement = (destiny - rb.position).normalized;
        agent.SetDestination(destiny);

        if (Mathf.Abs(Vector2.Distance(destiny, rb.position)) < 0.01f)
        {

            if (going)
            {
                patrolPosition++;
            }
            else
            {
                patrolPosition--;
            }
        }

        if (patrolPosition < 0)
        {
            going = true;
            patrolPosition = 1;
        }
        else if (patrolPosition >= nPoints)
        {
            if (cyclicPatrol)
                patrolPosition = 0;
            else
            {
                going = false;
                patrolPosition = nPoints-2;
            }
        }

        float Vx = movement.x;
        float Vy = movement.y;
        SetFlagsAnimation(Vx, Vy);
    }
}