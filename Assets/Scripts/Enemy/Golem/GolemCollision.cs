using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemCollision : MonoBehaviour
{
    [SerializeField] private GolemAttack golemAttack;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Nuke Plant")
        {
            golemAttack.targetInRange = true;
        }
        
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Nuke Plant")
        {
            golemAttack.targetInRange = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Nuke Plant")
        {
            golemAttack.targetInRange = false;
        }
    }
}
