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
    [SerializeField] private float atkDamage = 0f;
    
    //For attack function
    private float ticks = 0f;
    const float ATTACK_INTERVAL = 3.0f;

    private enum State { Chase, Damaged, ReachedPlayer, AttackPlayer };
    State currState = State.Chase;

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
        Action();
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

    private void Action()
    {
        switch (currState)
        {
            case State.Chase:
                Move();
                break;
            case State.ReachedPlayer:
                // To reset attack interval timer
                ticks = 0f;
                break;
            case State.AttackPlayer:
                Attack();
                break;
            case State.Damaged:
                // Change back color after receiving damage
                ChangeColor(Color.red);
                StartCoroutine("ResetColor", 0.2f);
                break;
        }
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

        currState = State.Damaged;
    }

    private void Die()
    {
        // +Add gold to player
        // +Contact spawn manager
        // +Drop loot

        PlayVFX(deathVFX, transform.position, transform.rotation, 1.0f); 

        EnemySpawningManager.instance.IncrementTotalEnemyKilledInWave();

        DestroyEnemy();
    }

    private void DestroyEnemy()
    {
        // Remove enemy game object from screen
        Vector3 currPosition = transform.position;
        currPosition.y = -20f;
        this.transform.position = currPosition;

        // Return gameobject to enemy pool
        EnemyPoolManager.instance.ReturnEnemy(this.gameObject);
    }

    private void Attack()
    {
        // Attack based on attack interval
        this.ticks += Time.deltaTime;
        if (ticks > ATTACK_INTERVAL)
        {
            PlayVFX(attackVFX, player.transform.position, Quaternion.LookRotation(player.transform.position - transform.position), 2.0f); 
            ticks = 0.0f;

            // +Player receives damage
        }
    }

    private void ChangeColor(Color color)
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    IEnumerator ResetColor(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        currState = State.Chase;
        ChangeColor(baseColor);
    }

    private void OnCollisionStay(Collision collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            currState = State.AttackPlayer;
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            currState = State.ReachedPlayer;
        }
    }

    private void OnCollisionExit(Collision collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            currState = State.Chase;
        }
    }

    private void PlayVFX(GameObject VFX, Vector3 position, Quaternion rotation, float ticks)
    {
        if (VFX)
        {
            Destroy(Instantiate(VFX, position, rotation), ticks);
        }
    }
}

