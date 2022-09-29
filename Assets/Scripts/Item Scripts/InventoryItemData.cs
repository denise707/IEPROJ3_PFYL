using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a scriptable object, that defines what an item is in our game.
/// It could be inherited from to have branched versions of items, for example potions and equipment.
/// </summary>


[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    [Header("Basic Properties")]
    public int ID;
    public string DisplayName;
    [TextArea(4, 4)]
    public string Description;
    public Sprite Icon;
    public int MaxStackSize;
    public GameObject prefab;

    [Header("Plant Properties")]
    public float growthDuration = 5;
    public List<Sprite> PlantGrowthSpriteList;

    public GameObject DropA;
    public GameObject DropB;

    [Header("Weapon Properties")]
    public float Damage = 5;
    //public List<Sprite> PlantGrowthSpriteList;

    //public GameObject DropA;
    //public GameObject DropB;

    //[Header("Tool Properties")]



}
