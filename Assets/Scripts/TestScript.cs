using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestScript : MonoBehaviour
{
    public GameObject Ant;

    private void OnEnable()
    {
        Ant = GameObject.FindGameObjectWithTag("Ant");
    }

    public void CloseUI()
    {
        Ant.GetComponent<RTSController>().isDisabled = false;
        GameObject currentSlug = Ant.GetComponent<RTSController>().currentSlug;
        currentSlug.GetComponent<NavMeshAgent>().isStopped = false;
        this.gameObject.SetActive(false);
    }
}
