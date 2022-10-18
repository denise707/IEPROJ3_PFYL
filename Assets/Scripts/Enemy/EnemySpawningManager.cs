using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawningManager : MonoBehaviour
{
    public static EnemySpawningManager instance;

    private List<Transform> spawnLocations = new List<Transform>();
    private List<String> enemyTypeList = new List<string>();
    private List<GameObject> spawnedEnemyList = new List<GameObject>();
    private bool releaseBoss = false;

    private enum SpawnerState { Generate, Release, EnemyCleared , Default};
    private SpawnerState currSpawnerState;

    private float ticks = 0.0f;
    private float minSpawnInterval = 3.0f;
    private float maxSpawnInterval = 10.0f;
    private float spawnInterval = 5.0f;

    private int difficulty = 1; //+Move to Game Manager 1 = -, 2 = easy...

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
        ticks = 0;
        enemyTypeList.Clear();
        releaseBoss = false;
        currSpawnerState = SpawnerState.Default;
    }

    private void RandomizeSpawnInterval()
    {
        spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }


    private void InitializeSpawnLocations()
    {
        for (int i = 0; i < 1; i++)
        {
            spawnLocations.Add(transform.GetChild(i).transform);
        }
    }

    void Update()
    {
        if (TimeManager.instance.IsNightTime())
        {
            if (currSpawnerState != SpawnerState.Release)
            {
                currSpawnerState = SpawnerState.Generate;
                RandomizeSpawnInterval();
            }
            
            switch (currSpawnerState)
            {
                case SpawnerState.Generate:
                    GenerateEnemies();
                    break;
                case SpawnerState.Release:
                    ReleaseEnemies();
                    break;
            }
        }

        else
        {
            if (currSpawnerState != SpawnerState.EnemyCleared)
            {
                ClearEnemies();
                currSpawnerState = SpawnerState.EnemyCleared;
            }
        }
    }

    private void GenerateEnemies()
    {
        currSpawnerState = SpawnerState.Release;

        switch (TimeManager.instance.day)
        {
            case 1: 
                if(!IsEnemyTypeExisting("Zombie"))
                    enemyTypeList.Add("Zombie");
                if(!IsEnemyTypeExisting("Weaker Boss Zombie"))
                    enemyTypeList.Add("Weaker Boss Zombie");
                break;
            case 2:
                if (!IsEnemyTypeExisting("Golem"))
                    enemyTypeList.Add("Golem");
                if (!IsEnemyTypeExisting("Weaker Boss Golem"))
                    enemyTypeList.Add("Weaker Boss Golem");
                break;
            case 3:
                if (!IsEnemyTypeExisting("Slime"))
                    enemyTypeList.Add("Slime");
                if (!IsEnemyTypeExisting("Weaker Boss Slime"))
                    enemyTypeList.Add("Weaker Boss Slime");
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

        for (int i = 0; i < enemyTypeList.Count; i++)
        {
            if (enemyType == enemyTypeList[i])
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

    private void ReleaseEnemies()
    {
        if (releaseBoss)
        {
            SpawnBossEnemy();
            releaseBoss = false;
        }

        this.ticks += Time.deltaTime;

        if (this.ticks > this.spawnInterval)
        {
            SpawnEnemy(GetRandomEnemyType());
            this.ticks = 0.0f;
        }
    }

    private string GetRandomEnemyType()
    {
        return enemyTypeList[Random.Range(0, enemyTypeList.Count)];
    }

    private void SpawnEnemy(string enemyName)
    {
        Transform location = this.spawnLocations[Random.Range(0, spawnLocations.Count)].transform;
        GameObject newEnemy = EnemyPoolManager.instance.GetEnemy(enemyName);
        newEnemy.transform.position = location.position;
        spawnedEnemyList.Add(newEnemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        spawnedEnemyList.Remove(enemy);
    }

    private void ClearEnemies()
    {
        for (int i = 0; i < spawnedEnemyList.Count; i++)
        {
            spawnedEnemyList[i].GetComponent<EnemyBehaviour>().DestroyEnemy();
        }
    }

    //--------------To be continued after implementing difficulty system--------------
    private void InitializeLevel()
    {
        // Spawn rate per difficulty
        switch (difficulty)
        {
            default:  break;
            // +max 
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
