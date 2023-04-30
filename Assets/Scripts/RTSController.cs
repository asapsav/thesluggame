using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RTSController : MonoBehaviour
{
    public NavMeshAgent antAgent;
    public bool isDisabled;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isDisabled)
        {
            Ray movePos = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(movePos, out var hitInfo))
            {
                antAgent.SetDestination(hitInfo.point);
            }
        }
    }
}
