using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{

    public GameObject enemy;
    public float spawnTime = 1f;
    public Transform[] spawnPoints;
    public int spawnLimit;

    public int[] spawnLimits;
    public int[] spawnHealth;
    public int[] waterAmount;
    public EnemyManagerEditor[] enemyEitor;

    public float timeBetweenNextWave = 10f;
    public int waveNum = 3;

    public int rainAmount = 0;

    private int spawnNum = 0;
    public int currentWave = 0;
    public int currentEnemyNum = 0;

    private float waveTime;
    //private float waitTime;

    public bool waveComing;

    // Use this for initialization
    void Start()
    {

        spawnTime = GameObject.Find("LevelEditor").GetComponent<LevelEditor>().EnemyManager_spawnTime;
        timeBetweenNextWave = GameObject.Find("LevelEditor").GetComponent<LevelEditor>().EnemyManager_timeBetweenNextWave;


        enemyEitor = GameObject.Find("LevelEditor").GetComponent<LevelEditor>().EnemyEditor;
        waveNum = enemyEitor.Length;
        spawnLimits = new int[waveNum];
        spawnHealth = new int[waveNum];
        waterAmount = new int[waveNum];
        for (int i = 0; i < waveNum; i++)
        {
            spawnLimits[i] = enemyEitor[i].spawnLimit;
            spawnHealth[i] = enemyEitor[i].spawnHealth;
            waterAmount[i] = enemyEitor[i].waterAmount;
        }

        waveComing = true;
        waveCome();
    }

    // Update is called once per frame
    void Update()
    {

        if (currentWave >= waveNum)
            return;

        if (!waveComing)
        {

            if (currentEnemyNum <= 0)
            {

                waveTime += Time.deltaTime;

                if (waveTime >= timeBetweenNextWave)
                {
                    waveComing = true;
                    waveCome();
                    waveTime = 0;
                }
            }
        }

    }

    void Spawn()
    {

        if (spawnNum >= spawnLimit)
        {
            CancelInvoke();
            spawnNum = 0;
            waveComing = false;
            return;
        }

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject currentEnemy = (GameObject)Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        currentEnemy.transform.GetComponent<EnemyHealth>().health = spawnHealth[currentWave - 1];
        currentEnemy.transform.GetComponent<EnemyInfo>().waterAmount = waterAmount[currentWave - 1];
        rainAmount += currentEnemy.transform.GetComponent<EnemyInfo>().waterAmount;

        spawnNum++;
        currentEnemyNum++;

    }

    void waveCome()
    {

        if (!waveComing)
            return;

        spawnLimit = spawnLimits[currentWave];
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        currentWave++;

    }

}
