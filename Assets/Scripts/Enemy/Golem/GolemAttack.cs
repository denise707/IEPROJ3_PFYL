using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GolemAttack : MonoBehaviour
{
    private EnemyBehaviour enemyBehaviour;
    private GolemRange golemRange;

    private float ticks = 0f;
    const float ATTACK_INTERVAL = 3.0f;

    public bool playerInRange = false;

    void Start()
    {
        enemyBehaviour = this.GetComponent<EnemyBehaviour>();
        golemRange = this.GetComponent<GolemRange>();
    }

    void Update()
    {
        if (enemyBehaviour.currState == EnemyBehaviour.State.AttackPlayer)
        {
            Attack();
        }
    }

    private void Attack()
    {
        this.ticks += Time.deltaTime;
        if (ticks > ATTACK_INTERVAL)
        {
            enemyBehaviour.currState = EnemyBehaviour.State.AttackPlayer;
            golemRange.HideRange();
            golemRange.ShowSpike();
            if (playerInRange) enemyBehaviour.AttackVFX();
            StartCoroutine("RemoveSpike");
            ticks = 0.0f;

            // +Player receives damage
        }
    }

    IEnumerator RemoveSpike()
    {
        yield return new WaitForSeconds(0.5f);
        enemyBehaviour.currState = EnemyBehaviour.State.Chase;
        golemRange.HideSpike();
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && enemyBehaviour.currState != EnemyBehaviour.State.AttackPlayer)
        {
            enemyBehaviour.currState = EnemyBehaviour.State.AttackPlayer;
            golemRange.showRange();
        }
    }
}
