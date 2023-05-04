using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

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

    IEnumerator RoundTimer()
    {
        yield return new WaitForSeconds(300f);
        StartEnemySpawns();
    }

    private void StartEnemySpawns()
    {

    }
}
