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
    public ItemType type;
    [TextArea(15,20)]
    public string description;
    //reference to Item
    public Item data = new Item();
}

[System.Serializable]

public class Item
{
    public string name;
    public int id = -1;
    public bool stackable;
    public Item(ItemObject item)
    {
        name = item.name;
        id = item.data.id;
        stackable = item.data.stackable;
    }
    public Item()
    {
        name = "";
        id = -1;
        stackable = false;
    }
}