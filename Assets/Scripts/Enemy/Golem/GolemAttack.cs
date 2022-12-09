using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class GolemAttack : MonoBehaviour
{
    private PlayerController playerController;
    private EnemyBehaviour enemyBehaviour;
    private GolemRange golemRange;

    private float ticks = 0f;
    const float ATTACK_INTERVAL = 3.5f;

    public bool targetInRange = false;
    public string targetTracked = " ";

    [Header("Sound Files")]
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private AudioClip spawnSFX;

    void Start()
    {
        enemyBehaviour = this.GetComponent<EnemyBehaviour>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
            if (targetInRange)
            {
                // Player receives damage
                if (enemyBehaviour.GetTarget() == "Player" && targetTracked == "Player")
                {
                    playerController.SetActiveTakeDamageEffect(true);
                    playerController.TakeDamage(enemyBehaviour.atkDamage);
                }
                // Nuke Plant receives damage
                if (enemyBehaviour.GetTarget() == "Nuke Plant")
                {
                    //Add code
                }
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
        yield return new WaitForSeconds(0.5f);
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
