using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public Inventory Container;
    public InventorySlot[] GetSlots { get { return Container.Slots; } }

    public bool AddItem(Item _item, int _amount)
    {
        InventorySlot slot = FindItemOnInventory(_item);
        if (EmptySlotCount <= 0)
        {
            //checks if the picked up item can be stacked inside one of the filled slots in either inventory
            if (database.itemObjects[_item.id].isStackable)
            {
                SetEmptySlot(_item, _amount);
                slot.AddAmount(_amount);
                return true;
            }
            else
                return false;
        }
        //if not stackable
        if (!database.itemObjects[_item.id].isStackable || slot == null)
        {
            SetEmptySlot(_item, _amount);
            return true;
        }
        slot.AddAmount(_amount);
        return true;
    }
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if(GetSlots[i].item.id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
    public InventorySlot FindItemOnInventory(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item.id == _item.id)
            {
                return GetSlots[i];
            }
        }
        return null;
    }
    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item.id <= -1)
            {
                GetSlots[i].UpdateSlot(_item, _amount);
                return GetSlots[i];
            }
        }
        // set up func for when inv is full
        return null;
    }
    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        /*if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }*/
        InventorySlot temp = new InventorySlot(item2.item, item2.amount);
        item2.UpdateSlot(item1.item, item1.amount);
        item1.UpdateSlot(temp.item, temp.amount);

    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if(GetSlots[i].item == _item)
            {
                GetSlots[i].UpdateSlot(null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.Slots[i].item, newContainer.Slots[i].amount);
            }
            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }
}

public delegate void SlotUpdated(InventorySlot _slot);

[System.Serializable]
public class InventorySlot
{
    [System.NonSerialized]
    public UserInterface parent;
    [System.NonSerialized] 
    public GameObject slotDisplay;
    [System.NonSerialized] 
    public SlotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;
    public Item item;
    public int amount;
    public ItemObject ItemObject
    {
        get
        {
            if(item.id >= 0)
            {
                return parent.inventory.database.itemObjects[item.id];
            }
            return null;
        }
    }
    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }
    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
    }
    public void UpdateSlot(Item _item, int _amount)
    {
        if(OnBeforeUpdate != null) { OnBeforeUpdate.Invoke(this); }

        item = _item;
        amount = _amount;

        if (OnAfterUpdate != null) { OnAfterUpdate.Invoke(this); }
    }
    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }

    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }
    //not working currently
    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if(_itemObject.data.id < 0 || _itemObject == null)
        {
            return true;
        }
        return false;
    }
}

[System.Serializable]
public class Inventory
{
    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            /*Items[i].UpdateSlot(new Item(), 0);*/
            Slots[i].RemoveItem();
        }
    }
    public InventorySlot[] Slots = new InventorySlot[18];
}