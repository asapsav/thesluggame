using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorcellCollection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ant")
        {
            GameManager.Instance.morcellAmount += 10;
            Destroy(gameObject);
        }
    }
}
