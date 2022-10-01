using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarSelectorManager : MonoBehaviour
{
    [SerializeField] GameObject hotbarContainer;
    public InventorySlot_UI []invSlots;
    private int slotIndex = 0;
    private PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        hotbarContainer = GameObject.FindGameObjectWithTag("Hotbar");
        invSlots = hotbarContainer.GetComponentsInChildren<InventorySlot_UI>();

        for (int i = 0; i < invSlots.Length; i++)
        {
          invSlots[i].name = $"[{i+1}] Slot";
          invSlots[i].UpdateUISlot();
        }

        invSlots[slotIndex].selector.SetActive(true);
        //UpdatePlayerEquip();

    }

    // Update is called once per frame
    void Update()
    {

        UpdateScrollSlot();
    }

    private void UpdatePlayerEquip()
    {
        if (invSlots[slotIndex].AssignedInventorySlot.ItemData != null)
        {
            Debug.Log(invSlots[slotIndex].AssignedInventorySlot.ItemData.DisplayName);
        }

        playerController.UpdatePlayerEquip(invSlots[slotIndex].AssignedInventorySlot.ItemData);
    }

    private void UpdateScrollSlot()
    {
        // if zero = no input
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                slotIndex++;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                slotIndex--;
            }

            if (slotIndex < 0)
            {
                slotIndex = invSlots.Length - 1;
            }
            else if (slotIndex > invSlots.Length - 1)
            {
                slotIndex = 0;
            }

            ResetSlotsSelector();
            invSlots[slotIndex].selector.SetActive(true);
            UpdatePlayerEquip();
        }
    }

    private void ResetSlotsSelector()
    {
        for (int i = 0; i < invSlots.Length; i++)
        {
            invSlots[i].selector.SetActive(false);
        }
    }
}
