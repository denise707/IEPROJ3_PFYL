using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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

    [SerializeField] private GameObject weakerBossSlimePrefab;
    [SerializeField] private GameObject weakerBossZombiePrefab;
    [SerializeField] private GameObject weakerBossGolemPrefab;

    [SerializeField] private GameObject bossSlimePrefab;
    [SerializeField] private GameObject bossZombiePrefab;
    [SerializeField] private GameObject bossGolemPrefab;

    //Prefab List
    private GameObject[] prefabList;

    //ENEMY POOL QUEUES
    private Queue<GameObject> slimePool = new Queue<GameObject>();
    private Queue<GameObject> zombiePool = new Queue<GameObject>();
    private Queue<GameObject> golemPool = new Queue<GameObject>();

    private Queue<GameObject> weakerBossSlimePool = new Queue<GameObject>();
    private Queue<GameObject> weakerBossZombiePool = new Queue<GameObject>();
    private Queue<GameObject> weakerBossGolemPool = new Queue<GameObject>();

    private Queue<GameObject> bossSlimePool = new Queue<GameObject>();
    private Queue<GameObject> bossZombiePool = new Queue<GameObject>();
    private Queue<GameObject> bossGolemPool = new Queue<GameObject>();

    //Pool List
    private Queue<GameObject>[] poolList;

    //INITIAL VALUES
    private int poolStartSize = 1;

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
        GameObject[] prefabList =
        {
            slimePrefab, zombiePrefab, golemPrefab,
            weakerBossSlimePrefab, weakerBossZombiePrefab, weakerBossGolemPrefab,
            bossSlimePrefab, bossZombiePrefab, bossGolemPrefab
        };

        Queue<GameObject>[] poolList =
        {
            slimePool, zombiePool, golemPool,
            weakerBossSlimePool, weakerBossZombiePool, weakerBossGolemPool,
            bossSlimePool, bossZombiePool, bossGolemPool
        };

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < poolStartSize; j++)
            {
                GameObject enemy = Instantiate(prefabList[i]);
                poolList[i].Enqueue(enemy);
                enemy.SetActive(false);
            }
        }
    }

    void Update()
    {

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

            case "Weaker Boss Slime":
                enemyPoolInfo = new EnemyPoolInfo(weakerBossSlimePool, weakerBossSlimePrefab); break;
            case "Weaker Boss Zombie":
                enemyPoolInfo = new EnemyPoolInfo(weakerBossZombiePool, weakerBossZombiePrefab); break;
            case "Weaker Boss Golem":
                enemyPoolInfo = new EnemyPoolInfo(weakerBossGolemPool, weakerBossGolemPrefab); break;

            case "Boss Slime":
                enemyPoolInfo = new EnemyPoolInfo(bossSlimePool, bossSlimePrefab); break;
            case "Boss Zombie":
                enemyPoolInfo = new EnemyPoolInfo(bossZombiePool, bossZombiePrefab); break;
            case "Boss Golem":
                enemyPoolInfo = new EnemyPoolInfo(bossGolemPool, bossGolemPrefab); break;
        }

        return enemyPoolInfo;
    }
}
