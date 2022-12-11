using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabbageBomb : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] GameObject mark;
    List<EnemyBehaviour> trackedEnemies = new List<EnemyBehaviour>();
    [SerializeField] GameObject vfx;
    [SerializeField] AudioClip bombSfx;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            trackedEnemies.Add(collider.gameObject.GetComponent<EnemyBehaviour>());
            mark.SetActive(true);
            StartCoroutine(Explode());
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            trackedEnemies.Remove(collider.gameObject.GetComponent<EnemyBehaviour>());
        }
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(2f);
        vfx.GetComponent<ParticleSystem>().Play();
        for (int i = 0; i < trackedEnemies.Count; i++)
        {
            trackedEnemies[i].ReceiveDamage(CombatManager.instance.damage);
        }
        AudioManager.instance.PlaySFX(bombSfx);
        Destroy(parent, 0.2f);
    }
}
