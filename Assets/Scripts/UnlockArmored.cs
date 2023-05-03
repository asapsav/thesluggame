using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockArmored : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ant")
        {
            GameManager.Instance.armoredUnlocked = true;
            Destroy(gameObject);
        }
    }
}
