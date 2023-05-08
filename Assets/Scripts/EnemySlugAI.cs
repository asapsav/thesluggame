using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySlugAI : MonoBehaviour
{
    public int slugHealth = 30;
    
    private NavMeshAgent enemyAgent;
    private Transform target;

    private float attackTimer = 3f;
    private float attackRange = 3.5f;

    private bool checkTarg;
    private bool hasTarget = false;

    enum AIStates
    {
        Moving,
        Chasing,
        Attacking
    }

    private AIStates currentState = AIStates.Moving;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
        Debug.Log(currentState);
        switch (currentState)
        {
            case AIStates.Moving:
                Moving();
                break;
            case AIStates.Chasing:
                Chasing();
                break;
            case AIStates.Attacking:
                Attacking();
                break;
            default:
                Debug.LogError("How did you get here?");
                break;
        }
    }

    private void Attacking()
    {
        if (target == null)
        {
            currentState = AIStates.Moving;
        }
        if (attackTimer <= 0)
        { 
            if (target.tag == "Slug")
            {
                checkTarg = target.GetComponent<SlugWanderAI>().TakeDamage(10);
            } else if (target.tag == "AntHill")
            {
                target.GetComponent<AntHillLogic>().TakeDamage(10);
            }
            attackTimer = 3;
        }
        if (checkTarg == true)
        {
            if (target.tag == "Slug")
            {
                Destroy(target.gameObject);
            }
            target = null;
            hasTarget = false;
            currentState = AIStates.Moving;
        }
    }

    private void Chasing()
    {
        if (target == null)
        {
            currentState = AIStates.Moving;
        }
        enemyAgent.SetDestination(target.transform.position);
        float distance = Vector3.Distance(target.transform.position, this.transform.position);
        if (target.tag == "AntHill")
        {
            attackRange = 10f;
        } else
        {
            attackRange = 3.5f;
        }
        if (distance <= attackRange)
        {
            Debug.Log("Attacking");
            currentState = AIStates.Attacking;
        }
    }

    private void Moving()
    {
        enemyAgent.SetDestination(new Vector3(0, 0, 0));
    }

    private void OnTriggerStay(Collider other)
    {
        if (hasTarget)
        {
            return;
        }
        if (other.tag == "Slug")
        {
            currentState = AIStates.Chasing;
            enemyAgent.SetDestination(other.transform.position);
            target = other.transform;
            hasTarget = true;
        } if (other.tag == "AntHill")
        {
            currentState = AIStates.Chasing;
            enemyAgent.SetDestination(other.transform.position);
            target = other.transform;
            hasTarget = true;
        }
    }

    public bool TakeDamage(int damage)
    {
        slugHealth -= damage;
        if (slugHealth <= 0)
        {
            return true;
        }
        return false;
    }
}
