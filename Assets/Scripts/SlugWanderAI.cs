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

    public bool isDisabled;

    public SlugTypes[] slugs;
    public SlugTypes currentSlug;

    public Material[] slugGlows;
    private Material currentMaterial;

    public GameObject slugInternal;

    private void Start()
    {
        slugAgent = GetComponent<NavMeshAgent>();
        currentSlug = slugs[0];
        currentMaterial = slugGlows[0];
        slugInternal.GetComponent<MeshRenderer>().material = currentMaterial;
    }

    private void Update()
    {
        Ray clickPos = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(clickPos, out var hit) && Input.GetMouseButtonDown(1))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Slug"))
            {
                GameObject fuckOff = hit.transform.gameObject;
                fuckOff.GetComponent<NavMeshAgent>().isStopped = true;
            }
        }
        if (isDisabled)
        {
            return;
        }
        switch (curStates)
        {
            case AIStates.Idle:
                DoIdle();
                break;
            case AIStates.Wandering:
                DoWander();
                break;
            default:
                Debug.LogError("How did you get here?");
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
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layerMask);

        return navHit.position;
    }

    public void ChangeSlug(int slugType)
    {
        currentSlug = slugs[slugType];
        currentMaterial = slugGlows[slugType];
        slugInternal.GetComponent<MeshRenderer>().material = currentMaterial;
    }
}
