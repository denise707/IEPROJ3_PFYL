using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Plant,
    Weapon,
    Tool
}

public abstract class ItemObject : ScriptableObject
{
    public Sprite uiDisplay;
    public bool isStackable = false;
    public int Id;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}

[System.Serializable]

public class Item
{
    public string name;
    public int id;
    public bool stackable;
    public Item(ItemObject item)
    {
        name = item.name;
        id = item.Id;
        stackable = item.isStackable;
    }
}