using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RTSController : MonoBehaviour
{
    public NavMeshAgent antAgent;
    public bool isDisabled;

    public GameObject biostaffUI;

    public GameObject currentSlug;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDisabled)
        {
            Ray movePos = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(movePos, out var hitInfo))
            {
                antAgent.SetDestination(hitInfo.point);
                if (hitInfo.transform.CompareTag("Slug"))
                {
                    currentSlug = hitInfo.transform.gameObject;
                    isDisabled = true;
                    biostaffUI.SetActive(true);
                }
            }
        }
    }
}
