using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;
    private InventoryItem item;

    public void Set(InventoryItem item)
    {
        this.item = item;

        if (item == null)
        {
            ClearSlot();
            return;
        }

        icon.enabled = true;
        icon.sprite = item.Item.Icon;
        amountText.text = item.Item.Stackable && item.Amount > 1
            ? item.Amount.ToString()
            : "";
    }

    public void ClearSlot()
    {
        icon.enabled = false;
        amountText.text = "";
        item = null;
    }

    public void OnSlotClicked()
    {
        if (item == null)
            return;

        EventBus.Publish(new InventorySlotClickedEvent(this));
    }

    public InventoryItem GetItemFromSlot() => item;
}
