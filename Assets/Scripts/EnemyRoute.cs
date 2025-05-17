using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyRoute : EnemyMoviment
{
    public List<Vector2> route = new List<Vector2>();
    private List<Vector2> routePoints = new List<Vector2>();
    private int patrolPosition;
    private bool going;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();

        
        routePoints.Add(rb.position);
        for (int i = 0; i < route.Count; i++)
            routePoints.Add(route[i]);

        target = "Patrol";
        going = true;

        animator.SetBool("IsWalking", true);
        animator.SetTrigger("Down");
    }

    public override void FixedUpdate() // Atualiza��o do animator e target
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
        Vector2 destiny = routePoints[patrolPosition];
        int nPoints = routePoints.Count;

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
            going = false;
            patrolPosition = patrolPosition-2;
        }

        float Vx = movement.x;
        float Vy = movement.y;
        SetFlagsAnimation(Vx, Vy);
    }
}