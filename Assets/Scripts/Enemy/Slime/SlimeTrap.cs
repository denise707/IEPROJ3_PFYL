using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrap : MonoBehaviour
{
    Collider collider;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<PlayerController>().SetSpeed(1f);
            this.collider = collider;
            StartCoroutine(RemoveSlow());
        }
    }

    IEnumerator RemoveSlow()
    {
        yield return new WaitForSeconds(2f);
        collider.gameObject.GetComponent<PlayerController>().SetSpeed(5f);
        Destroy(this.gameObject);
    }
}
