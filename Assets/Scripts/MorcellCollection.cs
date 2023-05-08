using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorcellCollection : MonoBehaviour
{
    public AudioSource PickUpSound;

    private void Start()
    {
        PickUpSound = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ant")
        {
            GameManager.Instance.morcellAmount += 10;
            StartCoroutine(PlaySoundAndDestroy());
        }
    }

    private IEnumerator PlaySoundAndDestroy()
    {
        PickUpSound.Play(0);
        yield return new WaitForSeconds(PickUpSound.clip.length);
        Destroy(gameObject);
    }
}
