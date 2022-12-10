using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornBush : MonoBehaviour
{
    [SerializeField] private PlantBehavior plantBehavior;
    public Sprite wilt;
    int charges = 3;

    private void OnTriggerEnter(Collider collider)
    {
        if (plantBehavior != null)
        {
            if (collider.gameObject.tag == "Enemy" && plantBehavior.plantName == "Thorn Bush" && charges > 0 && plantBehavior.plantStatus == PlantBehavior.PlantPhase.Phase3_FullyGrown)
            {
                collider.gameObject.GetComponent<EnemyBehaviour>().ReceiveDamage(10f);
                charges--; 
            }
        }

        if (charges <= 0)
        {
            plantBehavior.WiltPlant(wilt);
        }

        if (plantBehavior == null)
        {
            charges = 3;
        }
    }
}
