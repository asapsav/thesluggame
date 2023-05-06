using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlugWanderAI : MonoBehaviour
{
    enum AIStates
    {
        Idle,
        Wandering,
        MoveToRally,
        ReturnToRally,
        Chasing,
        Attack
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
    public GameObject Spawner;
    public GameObject dropPrefab;
    private MorcellSpawner morcellSpawner;

    public GameObject rallyPointN;
    public GameObject rallyPointS;
    private Transform target;
    private float attackRange = 3.5f;

    public TrailRenderer slugTrail;

    private int slugHealth;

    private bool slugDrop = false;
    public bool slugTypeChosen = false;

    private bool checkTarg;
    private float attackTimer = 3f;

    private bool hasTarget = false;

    public bool cancelled = false;

    private void Start()
    {
        slugAgent = GetComponent<NavMeshAgent>();
        currentSlug = slugs[0];
        currentMaterial = slugGlows[0];
        slugHealth = currentSlug.health;
        slugInternal.GetComponent<MeshRenderer>().material = currentMaterial;
        morcellSpawner = GetComponentInChildren<MorcellSpawner>();
        rallyPointN = GameObject.FindGameObjectWithTag("RallyPointN");
        rallyPointS = GameObject.FindGameObjectWithTag("RallyPointS");
        slugTrail = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        Ray clickPos = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(clickPos, out var hit) && Input.GetMouseButtonDown(0))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Slug"))
            {
                GameObject fuckOff = hit.transform.gameObject;
                fuckOff.GetComponent<NavMeshAgent>().isStopped = true;
            }
        }
        if (slugDrop == true)
        {
            StartCoroutine(SlugGrowTimer());
            slugDrop = false;
        }
        attackTimer -= Time.deltaTime;
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
            case AIStates.MoveToRally:
                //armored slugs move to rally point
                MoveToRally();
                break;
            case AIStates.ReturnToRally:
                //armored slugs wait at rally point
                break;
            case AIStates.Chasing:
                //armored slug chase enemy units and get within attack range
                Chasing();
                break;
            case AIStates.Attack:
                //armored slug kills enemy slugs, returns to rally if all others are dead
                Attack();
                break;
            default:
                Debug.LogError("How did you get here?");
                break;
        }
    }

    private void Attack()
    {
        if (target == null)
        {
            curStates = AIStates.MoveToRally;
        }
        if (attackTimer <= 0)
        {
            checkTarg = target.GetComponent<EnemySlugAI>().TakeDamage(10);
            attackTimer = 3f;
        }
        if (checkTarg == true)
        {
            Destroy(target.gameObject);
            target = null;
            hasTarget = false;
            curStates = AIStates.MoveToRally;
        }
    }

    private void Chasing()
    {
        if (target == null)
        {
            curStates = AIStates.MoveToRally;
        }
        slugAgent.SetDestination(target.transform.position);
        float distance = Vector3.Distance(target.transform.position, this.transform.position);
        if (distance <= attackRange)
        {
            Debug.Log("Attacking");
            curStates = AIStates.Attack;
        }
    }

    private void MoveToRally()
    {
        float distanceA = Vector3.Distance(rallyPointN.transform.position, this.gameObject.transform.position);
        float distanceB = Vector3.Distance(rallyPointS.transform.position, this.gameObject.transform.position);

        if (distanceA < distanceB)
        {
            slugAgent.SetDestination(rallyPointN.transform.position);
        } else
        {
            slugAgent.SetDestination(rallyPointS.transform.position);
        }
    }

    IEnumerator SlugGrowTimer()
    {
        yield return new WaitForSeconds(25f);
        if (!GameManager.Instance.armoredUnlocked)
        {
            Transform dropTransform = Spawner.GetComponent<Transform>();
            Instantiate(dropPrefab, dropTransform.position, dropTransform.rotation);
        }
    }

    private void DoIdle()
    {
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            return;
        }
        slugAgent.SetDestination(RandomNavSphere(transform.position, Random.Range(9f, 15f), floorMask));
        curStates = AIStates.Wandering;
    }

    private void DoWander()
    {
        if (slugAgent.pathStatus != NavMeshPathStatus.PathComplete)
            return;

        waitTimer = Random.Range(1f, 4f);
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
        if (slugTypeChosen)
        {
            return;
        }
        currentSlug = slugs[slugType];
        if (GameManager.Instance.morcellAmount < currentSlug.unitCost)
        {
            currentSlug = slugs[0];
            cancelled = true;
            return;
        }
        currentMaterial = slugGlows[slugType];
        slugInternal.GetComponent<MeshRenderer>().material = currentMaterial;
        Debug.Log(currentSlug.slugType);
        slugHealth = currentSlug.health;
        if (slugType == 1)
        {
            morcellSpawner.isGrowSlug = true;
            slugDrop = true;
            slugTrail.enabled = true;
        } else if (slugType == 2)
        {
            curStates = AIStates.MoveToRally;
            morcellSpawner.isGrowSlug = false;
        } else
        {
            morcellSpawner.isGrowSlug = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hasTarget)
        {
            return;
        }
        if (other.tag == "EnemySlug" && currentSlug.slugType == "Armored Slug")
        {
            slugAgent.SetDestination(other.transform.position);
            target = other.transform;
            curStates = AIStates.Chasing;
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
