using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSelectorManager : MonoBehaviour
{
    public static HotbarSelectorManager instance;


    [Header("HB Slots Properties")]
    [SerializeField] GameObject hotbarContainer; 
    public InventorySlot_UI []invSlots;
    private int slotIndex = 0;
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

        hotbarContainer = GameObject.FindGameObjectWithTag("Hotbar");
        invSlots = hotbarContainer.GetComponentsInChildren<InventorySlot_UI>(); // get array of hotbarinv slots

        for (int i = 0; i < invSlots.Length; i++)
        {
          invSlots[i].name = $"[{i+1}] Slot";
          //invSlots[i].UpdateUISlot();
        }

        invSlots[slotIndex].selector.SetActive(true); // show selector
        currInvSlot = invSlots[slotIndex];
        UpdatePlayerEquip(); // update player equip

    }

    // Update is called once per frame
    void Update()
    {

        UpdateScrollSlot(); // check scroll input
    }


    private void UpdateScrollSlot()
    {
        // if zero = no input
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                //scroll up
                slotIndex++;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                //scroll down
                slotIndex--;
            }

            // looping
            if (slotIndex < 0)
            {
                slotIndex = invSlots.Length - 1;
            }
            else if (slotIndex > invSlots.Length - 1)
            {
                slotIndex = 0;
            }

            // disable all selector
            ResetSlotsSelector();

            // show selector in current slot
            invSlots[slotIndex].selector.SetActive(true);
            currInvSlot = invSlots[slotIndex];

            // update player equipment (see func.)
            UpdatePlayerEquip();
        }
    }
    private void UpdatePlayerEquip()
    {
        // display item name in the slot if not empty
        if (invSlots[slotIndex].AssignedInventorySlot.ItemData != null)
        {
            Debug.Log(invSlots[slotIndex].AssignedInventorySlot.ItemData.DisplayName);
        }

        // update player equip by sending the item data to player controller
        playerController.UpdatePlayerEquip(invSlots[slotIndex].AssignedInventorySlot.ItemData);
    }
    private void ResetSlotsSelector()
    {
        for (int i = 0; i < invSlots.Length; i++)
        {
            invSlots[i].selector.SetActive(false);
        }
    }
}
