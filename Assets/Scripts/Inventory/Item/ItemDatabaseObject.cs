using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Item/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] items;
    public Dictionary<ItemObject, int> GetID = new Dictionary<ItemObject, int>();
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        //clearing dictionary to avoid duplications
        GetID = new Dictionary<ItemObject, int>();
        GetItem = new Dictionary<int, ItemObject>();
        //loop to add items into the new dictionary
        for(int i = 0; i < items.Length; i++)
        {
            GetID.Add(items[i], i);
            GetItem.Add(i, items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        
    }
}
