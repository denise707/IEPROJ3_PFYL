using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GolemAttack : MonoBehaviour
{
    private PlayerController playerController;
    private NukePlantBehavior nukeController;
    private EnemyBehaviour enemyBehaviour;
    private GolemRange golemRange;

    private float ticks = 0f;
    const float ATTACK_INTERVAL = 2f; //3.5

    public bool playerInRange = false;
    public bool nukeInRange = false;

    [Header("Sound Files")]
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private AudioClip spawnSFX;

    void Start()
    {
        enemyBehaviour = this.GetComponent<EnemyBehaviour>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        nukeController = GameObject.FindGameObjectWithTag("Nuke Plant").GetComponentInChildren<NukePlantBehavior>();
        golemRange = this.GetComponent<GolemRange>();
    }

    void OnEnable()
    {
        AudioManager.instance.PlaySFX(spawnSFX);
    }

    void Update()
    {
        if (enemyBehaviour.currState == EnemyBehaviour.State.AttackTarget)
        {
            Attack();
        }
    }

    private void Attack()
    {
        this.ticks += Time.deltaTime;
        if (ticks > ATTACK_INTERVAL)
        {
            ticks = 0.0f;

            // Player receives damage
            if (playerInRange)
            {
                playerController.SetActiveTakeDamageEffect(true);
                playerController.TakeDamage(enemyBehaviour.atkDamage);
            }
            // Nuke Plant receives damage
            if (nukeInRange)
            {
                nukeController.ReceiveDamage(enemyBehaviour.atkDamage);
            }
            
            AudioManager.instance.PlaySFX(attackSFX);

            enemyBehaviour.currState = EnemyBehaviour.State.AttackTarget;
            golemRange.HideRange();
            golemRange.ShowSpike();
            StartCoroutine("RemoveSpike");
        }
    }

    IEnumerator RemoveSpike()
    {
        yield return new WaitForSeconds(0.5f); //0.5
        enemyBehaviour.currState = EnemyBehaviour.State.Chase;
        golemRange.HideSpike();
    }

    private void OnCollisionStay(Collision collider)
    {
        if (collider.gameObject.tag == enemyBehaviour.GetTarget() && enemyBehaviour.currState != EnemyBehaviour.State.AttackTarget)
        {
            enemyBehaviour.currState = EnemyBehaviour.State.AttackTarget;
            golemRange.showRange();
        }
    }
}
