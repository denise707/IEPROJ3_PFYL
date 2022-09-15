using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawningManager : MonoBehaviour
{
    public static EnemySpawningManager instance;

    private List<Transform> spawnLocations = new List<Transform>();
    private List<String> enemyList = new List<string>();
    private bool releaseBoss = false;

    private enum SpawnerState { Generate, Release, WaveReleased, EnemyCleared };
    private SpawnerState currSpawnerState;

    private float ticks = 0.0f;
    private float SPAWN_INTERVAL = 5.0f;

    private int difficulty = 1; //+Move to Game Manager 1 = -, 2 = easy...

    private const int NUM_OF_WAVES = 5; //Max number of waves per day or level
    public static int currWave = 0; //Current wave #
    private int[] maxEnemyPerWave = { 0, 0, 0, 0, 0};

    private int waveMinEnemies = 1; //Min possible number of enemies per wave
    private int waveMaxEnemies = 1; //Max possible number of enemies per wave
    private int totalEnemyReleasedInWave = 0; //Total enemies per wave
    private int totalEnemyKilledInWave = 0; //Total enemies per wave

    public int totalEnemyKilledInLevel = 0;
    private int totalEnemyInLevel = 0;

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
        currWave = 0;
        totalEnemyReleasedInWave = 0;
        totalEnemyKilledInWave = 0;
        ticks = 0;
        enemyList.Clear();
        releaseBoss = false;
        currSpawnerState = SpawnerState.Generate;
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
            totalEnemyInLevel += maxEnemyPerWave[i]; //Add 1 more for boss
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

        switch (TimeManager.instance.day)
        {
            case 1: 
                if(!IsEnemyTypeExisting("Zombie"))
                    enemyList.Add("Zombie");
                if(currWave == 3 && !IsEnemyTypeExisting("Weaker Boss Zombie"))
                    enemyList.Add("Weaker Boss Zombie");
                break;
            case 2:
                if (!IsEnemyTypeExisting("Golem"))
                    enemyList.Add("Golem");
                if (currWave == 3 && !IsEnemyTypeExisting("Weaker Boss Golem"))
                    enemyList.Add("Weaker Boss Golem");
                break;
            case 3:
                if (!IsEnemyTypeExisting("Slime"))
                    enemyList.Add("Slime");
                if (currWave == 3 && !IsEnemyTypeExisting("Weaker Boss Slime"))
                    enemyList.Add("Weaker Boss Slime");
                releaseBoss = true;
                break;
            case 4:
                releaseBoss = true;
                break;
            case 5:
                releaseBoss = true;
                break;
        }
    }

    bool IsEnemyTypeExisting(String enemyType)
    {
        bool isExisting = false;

        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyType == enemyList[i])
            {
                isExisting = true;
            }
        }

        return isExisting;
    }

    private void SpawnBossEnemy()
    {
        switch (TimeManager.instance.day)  
        {
            case 3: 
                SpawnEnemy("Boss Zombie");
                break; 
            case 4:
                SpawnEnemy("Boss Golem");
                break;
            case 5:
                SpawnEnemy("Boss Slime");
                break;
        }
    }

    private void ReleaseWave()
    {
        if (currWave == 5 && releaseBoss)
        {
            maxEnemyPerWave[currWave] += 1; //Add 1 boss
            SpawnBossEnemy();
            releaseBoss = false;
        }

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
        totalEnemyInLevel = 0;
        totalEnemyKilledInLevel = 0;
        InitializeTotalEnemiesPerWave();
        currWave = 0;
    }

    private string GetRandomEnemyType()
    {
        //Debug.Log("Enemy type total: " + enemyList.Count);
        return enemyList[Random.Range(0, enemyList.Count)];
    }

    private void SpawnEnemy(string enemyName)
    {
        Transform location = this.spawnLocations[Random.Range(0, spawnLocations.Count)].transform;
        GameObject newEnemy = EnemyPoolManager.instance.GetEnemy(enemyName);
        newEnemy.transform.position = location.position;
    }

    public void IncrementTotalEnemyKilledInWave()
    {
        //Debug.Log(totalEnemyKilledInLevel + " / " + totalEnemyInLevel);
        totalEnemyKilledInLevel++;
        totalEnemyKilledInWave++;
    }

    public bool IsEnemyCleared()
    {
        return (currSpawnerState == SpawnerState.EnemyCleared) ? true : false;
    }

    public float GetEnemyKilledRatio()
    {
        //Debug.Log((float)totalEnemyKilledInLevel / totalEnemyInLevel);
        return (float)totalEnemyKilledInLevel / totalEnemyInLevel;
    }

    //--------------To be continued after implementing difficulty system--------------
    private void InitializeLevel()
    {
        // Spawn rate per difficulty
        switch (difficulty)
        {
            case 1: waveMinEnemies = 3; waveMaxEnemies = 5; break; //None
            case 2: waveMinEnemies = 5; waveMaxEnemies = 10; break; //Easy
            case 3: waveMinEnemies = 15; waveMaxEnemies = 20; break; //Medium
            case 4: waveMinEnemies = 15; waveMaxEnemies = 30; break; //Hard
            case 5: waveMinEnemies = 9999; waveMaxEnemies = 9999; break; //Impossible 
        }
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
}
