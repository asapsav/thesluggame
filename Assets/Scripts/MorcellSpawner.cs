using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorcellSpawner : MonoBehaviour
{
    private float spawnTimer = 0f;
    private Transform Spawner;
    public GameObject morcell;

    private void Start()
    {
        Spawner = GetComponent<Transform>();
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer > 3f)
        {
            Instantiate(morcell, transform.position, transform.rotation);
            spawnTimer = 0f;
        }
    }

}
