using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

struct EnemyPoolInfo
{
    public Queue<GameObject> enemyPool;
    public GameObject enemyPrefab;

    public EnemyPoolInfo(Queue<GameObject> pool, GameObject prefab)
    {
        this.enemyPool = pool;
        this.enemyPrefab = prefab;
    }
}

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager instance;

    [Header("ENEMY PREFABS")]
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private GameObject golemPrefab;

    //ENEMY POOL QUEUES
    private Queue<GameObject> slimePool = new Queue<GameObject>();
    private Queue<GameObject> zombiePool = new Queue<GameObject>();
    private Queue<GameObject> golemPool = new Queue<GameObject>();

    //INITIAL VALUES
    private int poolStartSize = 5;

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
        for (int i = 0; i < poolStartSize; i++)
        {
            GameObject slime = Instantiate(slimePrefab);
            slimePool.Enqueue(slime);
            slime.SetActive(false);
        }

        for (int i = 0; i < poolStartSize; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab);
            zombiePool.Enqueue(zombie);
            zombie.SetActive(false);
        }

        for (int i = 0; i < poolStartSize; i++)
        {
            GameObject golem = Instantiate(golemPrefab);
            golemPool.Enqueue(golem);
            golem.SetActive(false);
        }
    }

    void Update()
    {
        //Debug.Log("Slime Pool: " + slimePool.Count);
    }

    public GameObject GetEnemy(string enemyName)
    {
        EnemyPoolInfo enemyPoolInfo = GetEnemyOfDataType(enemyName);
        Queue<GameObject> enemyPool = enemyPoolInfo.enemyPool;
        GameObject enemyPrefab = enemyPoolInfo.enemyPrefab;

        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }

        else
        {
            GameObject enemy = Instantiate(enemyPrefab);
            return enemy;
        }
    }

    public void ReturnEnemy(GameObject enemy)
    {
        string enemyName = enemy.GetComponent<EnemyBehaviour>().enemyType;
        Queue<GameObject> enemyPool = GetEnemyOfDataType(enemyName).enemyPool;
        enemyPool.Enqueue(enemy);
        enemy.SetActive(false);
    }

    private EnemyPoolInfo GetEnemyOfDataType(string enemyName)
    {
        EnemyPoolInfo enemyPoolInfo = new EnemyPoolInfo();

        switch (enemyName)
        {
            case "Slime":
                enemyPoolInfo = new EnemyPoolInfo(slimePool, slimePrefab); break;
            case "Zombie":
                enemyPoolInfo = new EnemyPoolInfo(zombiePool, zombiePrefab); break;
            case "Golem":
                enemyPoolInfo = new EnemyPoolInfo(golemPool, golemPrefab); break;
        }

        return enemyPoolInfo;
    }
}
