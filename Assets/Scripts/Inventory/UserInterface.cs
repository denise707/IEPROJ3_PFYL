using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
public abstract class UserInterface : MonoBehaviour
{
    public InventoryObject inventory;
    //Game Object is the key
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    private void OnSlotUpdate(InventorySlot slot)
    {
        //checks if inv slot has an item
        if (slot.item.id >= 0)
        {
            //designed this way to make saving the inventory system lighter
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.ItemObject.uiDisplay;
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount == 1 ? "" : slot.amount.ToString("n0");
        }
        else
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            //sets alpha to 0
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        //temp, needs to only update when an item is added or removed
        //UpdateSlots();
    }

    //public void UpdateSlots()
    //{
    //    foreach (KeyValuePair<GameObject, InventorySlot> slot in slotsOnInterface)
    //    {
    //        //checks if inv slot has an item
    //        if (slot.Value.item.id >= 0)
    //        {
    //            //designed this way to make saving the inventory system lighter
    //            slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.ItemObject.uiDisplay;
    //            slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
    //            slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString("n0");
    //        }
    //        else
    //        {
    //            slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
    //            //sets alpha to 0
    //            slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
    //            slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
    //        }
    //    }
    //}

    public abstract void CreateSlots();
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj;
    }
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;
    }
    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseOver = obj.GetComponent<UserInterface>();
    }
    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseOver = null;
    }
    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItem != null)
        {
            MouseData.tempItem.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }
    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItem = CreateTemp(obj);
    }
    public GameObject CreateTemp(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].item.id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(75, 75);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplay;
            img.raycastTarget = false;
        }
        return tempItem;
    }
    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.tempItem);
        //if dragging item ended up outside any inventory, it destroys the item
        //to-do: implement either spawning dropped item or not destroying it altogether
        if(MouseData.interfaceMouseOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }
        //if we have an item in the slot we are dragging the current item to
        if (MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseOver.slotsOnInterface[MouseData.slotHoveredOver];
            inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }
    }
}
public static class MouseData
{
    public static UserInterface interfaceMouseOver;
    public static GameObject tempItem;
    public static GameObject slotHoveredOver;
}