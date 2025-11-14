using System;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemSO item;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        bool canAdd = InventoryManager.Instance.Inventory.CanAddItem(item, amount);
        if (!canAdd)
            return; 

        InventoryManager.Instance.AddItem(item, amount);
        Destroy(gameObject);
    }
}