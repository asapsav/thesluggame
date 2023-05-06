using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntHillLogic : MonoBehaviour
{
    public int health = 1000;
    public GameObject healthSliderCanvas;
    public Slider healthSlider;

    private void Update()
    {
        healthSliderCanvas.transform.LookAt(-Camera.main.transform.position);

        healthSlider.value = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("GAME OVER");
            GameManager.Instance.GameOver();
        }
    }
}
