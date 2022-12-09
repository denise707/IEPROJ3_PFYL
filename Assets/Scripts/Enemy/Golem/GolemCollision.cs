using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemCollision : MonoBehaviour
{
    [SerializeField] private GolemAttack golemAttack;

    private void OnTriggerEnter(Collider collider)
    {
        //if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Nuke Plant")
        //{
        //    golemAttack.targetInRange = true;
        //    golemAttack.targetTracked = collider.gameObject.tag;
        //}

        if (collider.gameObject.tag == "Player")
        {
            golemAttack.targetInRange = true;
            golemAttack.targetTracked = "Player";
        }

        if (collider.gameObject.tag == "Nuke Plant")
        {
            golemAttack.targetInRange = true;
            golemAttack.targetTracked = "Nuke Plant";
        }

    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            golemAttack.targetInRange = true;
            golemAttack.targetTracked = "Player";
        }

        if (collider.gameObject.tag == "Nuke Plant")
        {
            golemAttack.targetInRange = true;
            golemAttack.targetTracked = "Nuke Plant";
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            golemAttack.targetInRange = false;
            golemAttack.targetTracked = "Player";
        }

        if (collider.gameObject.tag == "Nuke Plant")
        {
            golemAttack.targetInRange = false;
            golemAttack.targetTracked = "Nuke Plant";
        }
    }
}
