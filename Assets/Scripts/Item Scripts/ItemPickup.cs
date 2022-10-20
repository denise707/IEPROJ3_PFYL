using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(SphereCollider))]*/

public class ItemPickup : MonoBehaviour
{
    public float PickupRadius = 1f;
    public InventoryItemData ItemData;


    private void Awake()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();

        if (!inventory) return;

        if (inventory.AddToInventory(ItemData, 1))
        {
            Destroy(this.gameObject);
        }
    }
}
