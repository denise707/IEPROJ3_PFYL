using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant Object", menuName = "Inventory System/Items/Plant")]
public class PlantObject : ItemObject
{
    public int growthSpeed = 1;
    public void Awake()
    {
        type = ItemType.Plant;
    }
}
