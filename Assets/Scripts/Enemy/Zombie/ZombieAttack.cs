using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    private PlayerController playerController;
    private NukePlantBehavior nukeController;
    private EnemyBehaviour enemyBehaviour;

    private float ticks = 0f;
    const float ATTACK_INTERVAL = 1.5f;

    [Header("Sound Files")] 
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private AudioClip spawnSFX;

    void Start()
    {
        enemyBehaviour = this.GetComponent<EnemyBehaviour>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        nukeController = GameObject.FindGameObjectWithTag("Nuke Plant").GetComponentInChildren<NukePlantBehavior>();
    }

    void OnEnable()
    {
        AudioManager.instance.PlaySFX(spawnSFX);
        ticks = 0.0f;
    }

    void Update()
    {
        switch (enemyBehaviour.currState)
        {
            case EnemyBehaviour.State.ReachedTarget:
                // To reset attack interval timer
                ticks = 0f;
                break;
            case EnemyBehaviour.State.AttackTarget:
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
            ticks = 0.0f;

            // Player receives damage
            if (enemyBehaviour.GetTarget() == "Player")
            {
                playerController.TakeDamage(enemyBehaviour.atkDamage);
                playerController.SetActiveTakeDamageEffect(true);
            }
            // Nuke Plant receives damage
            if (enemyBehaviour.GetTarget() == "Nuke Plant")
            {
                nukeController.ReceiveDamage(enemyBehaviour.atkDamage);
            }

            AudioManager.instance.PlaySFX(attackSFX);
        }
    }

    private void OnCollisionStay(Collision collider)
    {
        if (collider.gameObject.tag == enemyBehaviour.GetTarget())
        {
            enemyBehaviour.currState = EnemyBehaviour.State.AttackTarget;
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.tag == enemyBehaviour.GetTarget())
        {
            enemyBehaviour.currState = EnemyBehaviour.State.ReachedTarget;
        }
    }

    private void OnCollisionExit(Collision collider)
    {
        if (collider.gameObject.tag == enemyBehaviour.GetTarget())
        {
            enemyBehaviour.currState = EnemyBehaviour.State.Chase;
        }
    }
}
