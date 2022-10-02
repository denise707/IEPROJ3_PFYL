using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(SphereCollider))]

public class ItemPickup : MonoBehaviour
{
    public float PickupRadius = 1f;
    public InventoryItemData ItemData;

    private SphereCollider []myColliders;


    private void Awake()
    {
        myColliders = GetComponents<SphereCollider>();

       myColliders[0].isTrigger = true;
       myColliders[0].radius = PickupRadius;

       myColliders[1].radius = PickupRadius-0.01f;

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
