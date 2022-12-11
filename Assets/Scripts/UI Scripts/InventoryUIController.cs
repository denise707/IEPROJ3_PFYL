using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public DynamicInventoryDisplay chestPanel;
    public DynamicInventoryDisplay playerBackpackPanel;
    [SerializeField] private PlayerController player;

    private void Awake()
    {
        chestPanel.gameObject.SetActive(false);
        playerBackpackPanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested += DisplayPlayerBackpack;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested -= DisplayPlayerBackpack;
    }

    void Update()
    {
        //Keyboard check if 'ESC' was pressed, close both dynamic panels
        if (chestPanel.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            chestPanel.gameObject.SetActive(false);
            HotbarSelectorManager.instance.ActiveInventoryChecker();
            GameManager.instance.isInventory = false;
        }

        if (playerBackpackPanel.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            playerBackpackPanel.gameObject.SetActive(false);
            HotbarSelectorManager.instance.ActiveInventoryChecker();
            GameManager.instance.isInventory = false;
        }
    }

    void DisplayInventory(InventorySystem invToDisplay, int offset)
    {
        chestPanel.gameObject.SetActive(true);
        chestPanel.RefreshDynamicInventory(invToDisplay, offset);
    }

    void DisplayPlayerBackpack(InventorySystem invToDisplay, int offset)
    {
        playerBackpackPanel.gameObject.SetActive(true);
        playerBackpackPanel.RefreshDynamicInventory(invToDisplay, offset);
    }
}
