using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<InventorySlotUI> slots;

    [SerializeField] private GameObject deleteButton;
    private InventorySlotUI selectedSlot;
    private void OnEnable()
    {
        EventBus.Subscribe<InventoryUpdatedEvent>(Refresh);
        EventBus.Subscribe<InventorySlotClickedEvent>(OnSlotClicked);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<InventoryUpdatedEvent>(Refresh);
        EventBus.Unsubscribe<InventorySlotClickedEvent>(OnSlotClicked);
    }

    void Refresh(InventoryUpdatedEvent e)
    {
        var items = InventoryManager.Instance.Inventory.Items;

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Length)
                slots[i].Set(items[i]);   
            else
                slots[i].ClearSlot();     
        }
    }

    private void OnSlotClicked(InventorySlotClickedEvent e)
    {
        selectedSlot = e.Slot;

        deleteButton.SetActive(true);
        deleteButton.transform.position = selectedSlot.transform.position;
    }

    public void DeleteSelectedItem()
    {
        if (selectedSlot == null || selectedSlot.GetItemFromSlot() == null)
            return;

        InventoryManager.Instance.Inventory.RemoveItemStack(selectedSlot.GetItemFromSlot());

        deleteButton.SetActive(false);
        selectedSlot.ClearSlot(); 
        selectedSlot = null;

        EventBus.Publish(new InventoryUpdatedEvent());
    }

    private void Update() 
    { 
        if (deleteButton.activeSelf) 
        { 
            if (Input.GetMouseButtonDown(0)) 
            { 
                if (!IsPointerOverUI(deleteButton)) 
                { 
                    deleteButton.SetActive(false); 
                    selectedSlot = null; 
                } 
            } 
        } 
    }
    bool IsPointerOverUI(GameObject target) 
    { 
        return RectTransformUtility.RectangleContainsScreenPoint(
            target.GetComponent<RectTransform>(), 
            Input.mousePosition, 
            null); }

}
