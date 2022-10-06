using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] private InventoryItemData[] itemsToAdd;
    [SerializeField] protected int secondaryInventorySize;
    [SerializeField] protected InventorySystem secondaryInventorySystem;

    public InventorySystem SecondaryInventorySystem => secondaryInventorySystem;

    public static UnityAction<InventorySystem> OnPlayerBackpackDisplayRequested;

    protected override void Awake()
    {
        base.Awake();

        secondaryInventorySystem = new InventorySystem(secondaryInventorySize);

        for (int i = 0; i < itemsToAdd.Length; i++)
        {
            AddToInventory(itemsToAdd[i], 1);
        }

    }

    private void Start()
    {
        HotbarSelectorManager.instance.UpdatePlayerEquip(itemsToAdd[0]);

    }

    // Update is called once per frame
    void Update()
    {
        // keyboard check if 'B' key was pressed. preferred 'TAB' instead of 'B'
        if (Input.GetKeyDown(KeyCode.Tab)) 
        { 
            OnPlayerBackpackDisplayRequested?.Invoke(secondaryInventorySystem);
            HotbarSelectorManager.instance.ActiveInventoryChecker();

        }
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if(primaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }
        else if(secondaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        return false;
    }
}
