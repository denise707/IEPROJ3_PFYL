using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawningManager : MonoBehaviour
{
    public static EnemySpawningManager instance;

    private List<Transform> spawnLocations = new List<Transform>();

    private enum SpawnerState { Generate, Release, WaveReleased, EnemyCleared };
    private SpawnerState currSpawnerState;

    private float ticks = 0.0f;
    private float SPAWN_INTERVAL = 5.0f;

    private int difficulty = 1; //+Move to Game Manager

    private const int NUM_OF_WAVES = 6; //Max number of waves per day or level
    public static int currWave = 0; //Current wave #
    private int[] maxEnemyPerWave = { 0, 0, 0, 0, 0, 0 };

    private int waveMinEnemies = 5; //Min possible number of enemies per wave
    private int waveMaxEnemies = 10; //Max possible number of enemies per wave
    private int totalEnemyReleasedInWave = 0; //Total enemies per wave
    private int totalEnemyKilledInWave = 0; //Total enemies per wave

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        InitializeSpawnLocations();
        InitializeTotalEnemiesPerWave();
        currSpawnerState = SpawnerState.Generate;
    }

    private void InitializeLevel()
    {
        // +Spawn rate per difficulty
    }

    private void InitializeSpawnLocations()
    {
        for (int i = 0; i < 1; i++)
        {
            spawnLocations.Add(transform.GetChild(i).transform);
        }
    }

    private void InitializeTotalEnemiesPerWave()
    {
        for (int i = 0; i < NUM_OF_WAVES; i++)
        {
            maxEnemyPerWave[i] = Random.Range(waveMinEnemies, waveMaxEnemies + (i + 1));
        }
    }

    void Update()
    {
        if (TimeManager.instance.IsNightTime())
        {
            switch (currSpawnerState)
            {
                case SpawnerState.Generate:
                    GenerateWave();
                    break;
                case SpawnerState.Release:
                    ReleaseWave();
                    break;
                case SpawnerState.WaveReleased:
                    NextWave();
                    break;
            }

            //Debug.Log("TOTAL ENEMY IN WAVE = " + maxEnemyPerWave[currWave] + "/ TOTAL KILLED = " + totalEnemyKilledInWave);
        }

        else currSpawnerState = SpawnerState.Generate;
        Debug.Log(currSpawnerState);
    }

    private void GenerateWave()
    {
        currSpawnerState = SpawnerState.Release; 
        //maxEnemyPerWave[0] = 1; ->For debugging

        // +Depending of day (ex. day 5 is boss day) and difficulty
        // +If boss wave or weaker boss wave, spawn here then maxEnemyPerWave[currWave]--;
        // +Add weaker boss to enemy pool

        //Debug.Log("CURRENT WAVE = " + (currWave + 1) + "/ TOTAL WAVES = " + NUM_OF_WAVES);
    }

    private void ReleaseWave()
    {
        if (totalEnemyReleasedInWave < maxEnemyPerWave[currWave])
        {
            this.ticks += Time.deltaTime;

            if (this.ticks > this.SPAWN_INTERVAL)
            {
                this.SPAWN_INTERVAL = Random.Range(5.0f, 10.0f);
                SpawnEnemy(GetRandomEnemyType());
                totalEnemyReleasedInWave++;
                this.ticks = 0.0f;
            }
        }

        else
        {
            currSpawnerState = SpawnerState.WaveReleased;
            totalEnemyReleasedInWave = 0;
        }
    }

    private void NextWave()
    {
        if (totalEnemyKilledInWave == maxEnemyPerWave[currWave])
        {
            currWave++;
            totalEnemyKilledInWave = 0;
            currSpawnerState = SpawnerState.Generate;
            if (currWave >= NUM_OF_WAVES)
            {
                currSpawnerState = SpawnerState.EnemyCleared;
                NextDay();
            }
        }
    }

    private void NextDay()
    {
        InitializeTotalEnemiesPerWave();
        currWave = 0;
    }

    private string GetRandomEnemyType()
    {
        string[] enemyList = { "Slime", "Zombie", "Golem"};
        return enemyList[Random.Range(0, enemyList.Length)];
    }

    private void SpawnEnemy(string enemyName)
    {
        Transform location = this.spawnLocations[Random.Range(0, spawnLocations.Count)].transform;
        GameObject newEnemy = EnemyPoolManager.instance.GetEnemy(enemyName);
        newEnemy.transform.position = location.position;
    }

    public void IncrementTotalEnemyKilledInWave()
    {
        totalEnemyKilledInWave++;
    }

    public void IncreaseEnemyStats()
    {
        // +For different difficulty
    }

    public bool IsEnemyCleared()
    {
        return (currSpawnerState == SpawnerState.EnemyCleared) ? true : false;
        Debug.Log("Enemy cleared");
    }
}
