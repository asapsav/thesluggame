using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text morcellCount;
    private float countdownTimer = 150f;

    //Stuff for spawning enemies
    [SerializeField] private GameObject tigerSlugPref;
    [SerializeField] private GameObject wolfSlugPref;
    [SerializeField] private Transform[] tigerSlugSpawns;
    [SerializeField] private Transform[] wolfSlugSpawns;

    public GameObject GameOverPanel;

    private bool isSpawningWaves;
    private float spawnTimer = 0f;
    private int wavesSurvived;
    private bool gameOver;

    public TMP_Text gameOverSplash;

    private void Awake()
    {
        _instance = this;
    }

    public bool armoredUnlocked = false;
    public int morcellAmount = 1000;

    private void Start()
    {
        StartCoroutine(RoundTimer());
    }

    private void Update()
    {
        morcellCount.text = "Morcell: " + morcellAmount;
        if (gameOver)
        {
            return;
        }
        if (countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
        } else
        {
            countdownTimer = 0;
        }
        DisplayTime(countdownTimer);
        if (isSpawningWaves)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0)
            {
                SpawnWave();
                spawnTimer = 40f;
                wavesSurvived++;
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timer.text = "Time until enemies arrive: \n" + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator RoundTimer()
    {
        yield return new WaitForSeconds(150f);
        Debug.Log("Spawning first wave");
        isSpawningWaves = true;
    }

    private void SpawnWave()
    {
        for (int i = 0; i < tigerSlugSpawns.Length; i++)
        {
            Instantiate(tigerSlugPref, tigerSlugSpawns[i].position, tigerSlugSpawns[i].rotation);
        }
        for (int i = 0; i < wolfSlugSpawns.Length; i++)
        {
            Instantiate(wolfSlugPref, wolfSlugSpawns[i].position, wolfSlugSpawns[i].rotation);
        }
    }

    public void GameOver()
    {
        gameOver = true;
        GameOverPanel.SetActive(true);
        gameOverSplash.text = "You survived for " + wavesSurvived + " waves!";
    }
}
