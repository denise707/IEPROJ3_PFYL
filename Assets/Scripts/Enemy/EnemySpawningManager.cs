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

    private int difficulty = 1; //+Move to Game Manager 1 = -, 2 = easy...

    private const int NUM_OF_WAVES = 6; //Max number of waves per day or level
    public static int currWave = 0; //Current wave #
    private int[] maxEnemyPerWave = { 0, 0, 0, 0, 0, 0 };

    private int waveMinEnemies = 0; //Min possible number of enemies per wave
    private int waveMaxEnemies = 0; //Max possible number of enemies per wave
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
        InitializeLevel();
        InitializeTotalEnemiesPerWave();
        currSpawnerState = SpawnerState.Generate;
    }

    private void InitializeLevel() //To be continued after implementing difficulty system
    {
        // Spawn rate per difficulty
        switch (difficulty)
        {
            case 1: waveMinEnemies = 3; waveMaxEnemies = 5; break; //None
            case 2: waveMinEnemies = 5; waveMaxEnemies = 10; break; //Easy
            case 3: waveMinEnemies = 15; waveMaxEnemies = 20; break; //Medium
            case 4: waveMinEnemies = 15; waveMaxEnemies = 30; break; //Hard
            case 5: waveMinEnemies = 9999; waveMaxEnemies = 9999; break; //Impossible (WIP)
        }
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
            maxEnemyPerWave[i] = Random.Range(waveMinEnemies, waveMaxEnemies);
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

        }

        else currSpawnerState = SpawnerState.Generate;
    }

    private void GenerateWave()
    {
        if(TimeManager.instance.IsNightTime())
            currSpawnerState = SpawnerState.Release;
        //maxEnemyPerWave[0] = 1; ->For debugging
        Debug.Log("Curr Wave " + currWave + 1);
 
        switch (TimeManager.instance.day)
        {
            // Boss waves on Day 5
            case 5: SpawnBossEnemy(); break;
            // Possible to spawn weaker boss enemies
            case 6: 
            case 7: if (Random.Range(1, 100) >= 80) { SpawnEnemy("Weaker Boss " + GetRandomEnemyType()); } break;
        }
    }

    private void SpawnBossEnemy()
    {
        switch (difficulty) // Different spawn rate according to difficulty
        {
            case 1: if ((currWave + 1) % 5 == 0) { SpawnEnemy("Boss " + GetRandomEnemyType()); } break; //None
            case 2: if ((currWave + 1) % 5 == 0) { SpawnEnemy("Boss " + GetRandomEnemyType()); } break; //Easy
            case 3: if ((currWave + 1) % 3 == 0) { SpawnEnemy("Boss " + GetRandomEnemyType()); } break; //Medium
            case 4: if ((currWave + 1) % 2 == 0) { SpawnEnemy("Boss " + GetRandomEnemyType()); } break; //Hard
            case 5: SpawnEnemy("Boss " + GetRandomEnemyType()); break; //Impossible
        }
    }

    private void ReleaseWave()
    {
        Debug.Log("total = " + totalEnemyReleasedInWave + " / max = " + maxEnemyPerWave[currWave]);
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

    public float IncreaseEnemyStats(float baseHP)
    {
        // For different difficulty
        float additionalHP = 0;
        switch (difficulty)
        {
            case 1: additionalHP = 0; break; //None
            case 2: additionalHP = 0; break; //Easy
            case 3: additionalHP = baseHP * 0.10f; break; //Medium
            case 4: additionalHP = baseHP * 0.20f; break; //Hard
            case 5: additionalHP = baseHP; break; //Impossible
        }

        return additionalHP;
    }

    public bool IsEnemyCleared()
    {
        return (currSpawnerState == SpawnerState.EnemyCleared) ? true : false;
    }
}
