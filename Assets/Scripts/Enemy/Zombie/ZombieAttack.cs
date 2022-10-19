using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    private PlayerController playerController;

    private EnemyBehaviour enemyBehaviour;
    private GolemRange golemRange;

    private float ticks = 0f;
    const float ATTACK_INTERVAL = 3.0f;

    [Header("Sound Files")] 
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private AudioClip spawnSFX;

    void Start()
    {
        enemyBehaviour = this.GetComponent<EnemyBehaviour>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        playerController = player.GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        AudioManager.instance.PlaySFX(spawnSFX);
    }

    void Update()
    {
        switch (enemyBehaviour.currState)
        {
            case EnemyBehaviour.State.ReachedPlayer:
                // To reset attack interval timer
                ticks = 0f;
                break;
            case EnemyBehaviour.State.AttackPlayer:
                Attack();
                break;
        }
    }

    private void Attack()
    {
        // Attack based on attack interval
        this.ticks += Time.deltaTime;
        if (ticks > ATTACK_INTERVAL)
        {
            enemyBehaviour.AttackVFX();
            ticks = 0.0f;

            // +Player receives damage
            playerController.TakeDamage(enemyBehaviour.atkDamage);
            AudioManager.instance.PlaySFX(attackSFX);
        }
    }

    private void OnCollisionStay(Collision collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            enemyBehaviour.currState = EnemyBehaviour.State.AttackPlayer;
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            enemyBehaviour.currState = EnemyBehaviour.State.ReachedPlayer;
        }
    }

    private void OnCollisionExit(Collision collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            enemyBehaviour.currState = EnemyBehaviour.State.Chase;
        }
    }
}
