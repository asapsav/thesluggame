using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorcellSpawner : MonoBehaviour
{
    private float spawnTimerMorcell = 0f;
    private float spawnTimerSS = 0f;
    public GameObject morcell;
    public bool isGrowSlug = false;

    public GameObject stemSlug;

    private void Update()
    {
        if (isGrowSlug)
        {
            spawnTimerMorcell += Time.deltaTime;
            spawnTimerSS += Time.deltaTime;
            if (spawnTimerMorcell > 3f)
            {
                Instantiate(morcell, transform.position, transform.rotation);
                spawnTimerMorcell = 0f;
            }
            if (spawnTimerSS > 20f)
            {
                GameObject newSlug = Instantiate(stemSlug, transform.position, transform.rotation);
                newSlug.GetComponentInChildren<MorcellSpawner>().isGrowSlug = false;
                newSlug.GetComponent<SlugWanderAI>().slugTrail.enabled = false;
                newSlug.GetComponent<SlugWanderAI>().slugTypeChosen = false;
                spawnTimerSS = 0f;
            }
        }
    }
}
