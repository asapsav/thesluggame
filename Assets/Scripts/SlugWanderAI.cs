using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlugWanderAI : MonoBehaviour
{
    enum AIStates
    {
        Idle,
        Wandering
    }

    [SerializeField] private NavMeshAgent slugAgent = null;
    [SerializeField] private LayerMask floorMask = 0;

    private AIStates curStates = AIStates.Idle;
    private float waitTimer = 0.0f;
    private float spawnTimer = 0f;

    public bool isDisabled;

    private void Start()
    {
        slugAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        switch (curStates)
        {
            case AIStates.Idle:
                DoIdle();
                break;
            case AIStates.Wandering:
                DoWander();
                break;
            default:
                Debug.LogError("GUH?!");
                break;
        }
    }

    private void DoIdle()
    {
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            return;
        }
        slugAgent.SetDestination(RandomNavSphere(transform.position, 10.0f, floorMask));
        curStates = AIStates.Wandering;
    }

    private void DoWander()
    {
        if (slugAgent.pathStatus != NavMeshPathStatus.PathComplete)
            return;

        waitTimer = Random.Range(1.0f, 4.0f);
        curStates = AIStates.Idle;
    }

    Vector3 RandomNavSphere(Vector3 origin, float distance, LayerMask layerMask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layerMask);

        return navHit.position;
    }
}
