using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum state
    {
        patrol,
        interest,
        chase
    }

    [Header("State Machine")]
    public state enemyState;

    [Header("State Speeds")]
    public float patrolSpeed;
    public float interestSpeed;
    public float chaseSpeed;

    [Header("Target")]
    public Transform interestTarget;
    public Transform chaseTarget;

    private void Update()
    {
        if(enemyState == state.patrol)
        {
            Patrol();
        }
        else if(enemyState == state.interest)
        {
            Interest();
        }
        else if (enemyState == state.chase)
        {
            Chase();
        }
    }

    void Patrol()
    {
        transform.Translate(Vector3.forward * patrolSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * 100f * Time.deltaTime);
    }

    void Interest()
    {
        transform.position = Vector3.MoveTowards(transform.position, interestTarget.position, interestSpeed * Time.deltaTime);
    }

    void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, chaseTarget.position, chaseSpeed * Time.deltaTime);
    }
}
