using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("STATUS")]
    [SerializeField] public string enemyType = "";
    private float speed = 0f;
    private float currHealth = 0f;
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] public float atkDamage = 0f;
    [SerializeField] private int gold = 0;

    public enum State { Chase, Damaged, ReachedPlayer, AttackPlayer };
    public State currState = State.Chase;

    [Header("VFX")]
    [SerializeField] GameObject spawnVFX;
    [SerializeField] GameObject attackVFX;
    [SerializeField] GameObject deathVFX;

    private GameObject player;
    private Animator anim;

    //For sprite color 
    private Color baseColor;

    private void Start()
    {
        // For different difficulty 
        maxHealth += EnemySpawningManager.instance.IncreaseEnemyStats(currHealth);
        InitializeEnemy();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        PlayVFX(spawnVFX, transform.position, transform.rotation, 2.0f); 
    }

    private void Update()
    {
        if (currState == State.Chase) Move();
    }

    void OnEnable()
    {
        InitializeEnemy();
    }

    void OnDisable()
    {
        ChangeColor(baseColor);
    }

    private void InitializeEnemy()
    {
        speed = Random.Range(0.5f, 3.0f);
        currHealth = maxHealth;
        GetComponentInChildren<EnemyHPBar>().UpdateHPBar(currHealth, maxHealth);
        currState = State.Chase;
        baseColor = GetComponent<SpriteRenderer>().color;
    }

    private void Move()
    {
        // Follow player
        Transform playerTransform = player.transform; 
        this.transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

        UpdateAnimation(playerTransform);
    }

    private void UpdateAnimation(Transform playerTransform)
    {
        // Update sprite based on player direction
        Vector3 dir = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        Vector2 movement = dir;

        // Update animator parameters
        anim.SetFloat("X", dir.x);
        anim.SetFloat("Y", dir.z);
    }

    public void ReceiveDamage(float damage)
    {
        // Subtract HP
        this.currHealth -= damage;

        // Update HP Bar
        GetComponentInChildren<EnemyHPBar>().UpdateHPBar(currHealth, maxHealth); 

        if (this.currHealth <= 0)
        {
            Die();
        }

        Damaged();
    }

    private void Die()
    {
        PlayerData.instance.ReceiveGold(this.gold);

        // +Drop loot

        EnemySpawningManager.instance.RemoveEnemy(this.gameObject);

        DestroyEnemy();
    }

    public void DestroyEnemy()
    {
        PlayVFX(deathVFX, transform.position, transform.rotation, 1.0f);

        // Remove enemy game object from screen
        Vector3 currPosition = transform.position;
        currPosition.y = -20f;
        this.transform.position = currPosition;

        // Return gameobject to enemy pool
        EnemyPoolManager.instance.ReturnEnemy(this.gameObject);
    }

    private void Damaged()
    {
        ChangeColor(Color.red);
        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine("ResetColor", 0.2f);
        }
        else
        {
            ChangeColor(baseColor);
        }
    }

    private void ChangeColor(Color color)
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    IEnumerator ResetColor(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ChangeColor(baseColor);
    }


    private void PlayVFX(GameObject VFX, Vector3 position, Quaternion rotation, float ticks)
    {
        if (VFX)
        {
            Destroy(Instantiate(VFX, position, rotation), ticks);
        }
    }

    public void AttackVFX()
    {
        PlayVFX(attackVFX, player.transform.position, Quaternion.LookRotation(player.transform.position - transform.position), 2.0f);
    }
}

