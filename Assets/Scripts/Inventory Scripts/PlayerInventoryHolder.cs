using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] private InventoryItemData[] itemsToAdd;

    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnPlayerBackpackDisplayRequested;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < itemsToAdd.Length; i++)
        {
            AddToInventory(itemsToAdd[i], 1);
        }

    }

    private void Start()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);

        HotbarSelectorManager.instance.UpdatePlayerEquip(itemsToAdd[0]);
    }

    protected override void LoadInventory(SaveData data)
    {
        // Check the save data for this specific chests inventory, and if it exists, load it in.
        if (data.playerInventory.InvSystem != null)
        {
            this.primaryInventorySystem = data.playerInventory.InvSystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }

  

    // Update is called once per frame
    void Update()
    {
        // keyboard check if 'B' key was pressed. preferred 'TAB' instead of 'B'
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            OnPlayerBackpackDisplayRequested?.Invoke(primaryInventorySystem, 8);
            HotbarSelectorManager.instance.ActiveInventoryChecker();

            GameManager.instance.isInventory = true;
        }
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if(primaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        return false;
    }
}
