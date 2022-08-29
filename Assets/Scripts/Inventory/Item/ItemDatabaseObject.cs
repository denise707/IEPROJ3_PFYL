using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Item/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] itemObjects;

    public void UpdateID()
    {
        //setting item id
        for (int i = 0; i < itemObjects.Length; i++)
        {
            if (itemObjects[i].data.id != i)
            {
                itemObjects[i].data.id = i;
            }
        }
    }
    public void OnAfterDeserialize()
    {
        UpdateID();
    }

    public void OnBeforeSerialize()
    {
    }
}
