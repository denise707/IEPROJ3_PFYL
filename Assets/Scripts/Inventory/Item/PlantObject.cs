using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant Object", menuName = "Inventory System/Items/Plant")]

public class PlantObject : ItemObject
{
    public float growthDuration = 5;
    public List<Sprite> PlantGrowthSpriteList;

    public Sprite DropsA;
    public Sprite DropsB;

    public GameObject prefab;


    public void Awake()
    {
        
        isStackable = true;
    }
}
