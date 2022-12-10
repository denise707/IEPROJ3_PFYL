using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemCollision : MonoBehaviour
{
    [SerializeField] private GolemAttack golemAttack;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            golemAttack.playerInRange = true;
        }

        if (collider.gameObject.tag == "Nuke Plant")
        {
            golemAttack.nukeInRange = true;
        }

    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            golemAttack.playerInRange = true;
        }

        if (collider.gameObject.tag == "Nuke Plant")
        {
            golemAttack.nukeInRange = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            golemAttack.playerInRange = false;
        }

        if (collider.gameObject.tag == "Nuke Plant")
        {
            golemAttack.nukeInRange = false;
        }
    }
}
