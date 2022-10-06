using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HotbarSelectorManager : MonoBehaviour
{
    public static HotbarSelectorManager instance;


    [Header("HB Slots Properties")]
    [SerializeField] GameObject hotbarContainer;
    [SerializeField] InventoryUIController invController;

    public InventorySlot_UI []invSlots;
    private int currentSlotIndex = 0;
    private PlayerController playerController;

    public InventorySlot_UI currInvSlot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); // get player controller
        invController = GameObject.FindGameObjectWithTag("InventoryController").GetComponent<InventoryUIController>(); // get player controller


        hotbarContainer = GameObject.FindGameObjectWithTag("Hotbar");
        invSlots = hotbarContainer.GetComponentsInChildren<InventorySlot_UI>(); // get array of hotbarinv slots

        for (int i = 0; i < invSlots.Length; i++)
        {
          invSlots[i].name = $"[{i+1}] Slot";
          invSlots[i].UpdateUISlot();
        }

        invSlots[currentSlotIndex].selector.SetActive(true); // show selector
        currInvSlot = invSlots[currentSlotIndex];
        UpdatePlayerEquip(); // update player equip

        ActiveInventoryChecker();

    }

    // Update is called once per frame
    void Update()
    {

        UpdateScrollSlot(); // check scroll input
    }

    public void ActiveInventoryChecker()
    {
        if(invController.chestPanel.isActiveAndEnabled ||invController.playerBackpackPanel.isActiveAndEnabled)
        {
            //Debug.Log("has active inv");
            SetHotbarButtonsState(true);
        }
        else
        {
            //Debug.Log("no active inv");

            SetHotbarButtonsState(false);
        }
    }

    private void SetHotbarButtonsState(bool flag)
    {
        for (int i = 0; i < invSlots.Length; i++)
        {
            invSlots[i].GetComponent<Button>().interactable = flag;
        }
    }
    private void UpdateScrollSlot()
    {
        // if zero = no input
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                //scroll up
                currentSlotIndex--;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                //scroll down
                currentSlotIndex++;
            }

            // looping
            if (currentSlotIndex < 0)
            {
                currentSlotIndex = invSlots.Length - 1;
            }
            else if (currentSlotIndex > invSlots.Length - 1)
            {
                currentSlotIndex = 0;
            }

            // disable all selector
            ResetSlotsSelector();

            // show selector in current slot
            invSlots[currentSlotIndex].selector.SetActive(true);
            currInvSlot = invSlots[currentSlotIndex];

            // update player equipment (see func.)
            UpdatePlayerEquip();
        }
    }
    public void UpdatePlayerEquip()
    {
        // display item name in the slot if not empty
        if (invSlots[currentSlotIndex].AssignedInventorySlot.ItemData != null)
        {
            Debug.Log(invSlots[currentSlotIndex].AssignedInventorySlot.ItemData.DisplayName);
            Debug.Log("Update Equip");

        }

        // update player equip by sending the item data to player controller
        playerController.UpdatePlayerEquip(invSlots[currentSlotIndex].AssignedInventorySlot.ItemData);
    }

    public void UpdatePlayerEquip(InventoryItemData data)
    {
        // display item name in the slot if not empty
        if (data != null)
        {
            Debug.Log(data.DisplayName);
            //Debug.Log("Update Equip");

        }

        // update player equip by sending the item data to player controller
        playerController.UpdatePlayerEquip(data);
    }
    private void ResetSlotsSelector()
    {
        for (int i = 0; i < invSlots.Length; i++)
        {
            invSlots[i].selector.SetActive(false);
        }
    }
}
