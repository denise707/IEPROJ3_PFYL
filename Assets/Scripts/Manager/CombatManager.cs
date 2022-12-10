using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    GameObject target = null;

    [SerializeField] private GameObject bombCopy;
    [SerializeField] private GameObject spawnLoc;

    public float damage = 0;

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

    public void UseRoseDagger(float damage)
    {
        if(target != null)
        {
            target.GetComponent<EnemyBehaviour>().ReceiveDamage(damage);
            HotbarSelectorManager.instance.currInvSlot.RemoveFromStack(1);
        }        
    }

    public void UseCabbageBomb(float damage)
    {
        this.damage = damage;
        GameObject newTrap = Instantiate(bombCopy, spawnLoc.transform.position, bombCopy.transform.rotation);
        HotbarSelectorManager.instance.currInvSlot.RemoveFromStack(1);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            target = other.gameObject;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            target = other.gameObject;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        target = null;
    }
}
