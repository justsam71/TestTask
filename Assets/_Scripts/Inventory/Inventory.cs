using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Inventory
{
    private int maxSlots = 4;
    public InventoryItem[] Items;

    public Inventory(int maxSlots)
    {
        this.maxSlots = maxSlots;
        Items = new InventoryItem[maxSlots];
    }

    public bool AddItem(ItemSO item, int amount = 1)
    {
        if (item.Stackable)
        {
            int remaining = amount;

            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null && Items[i].Item.Id == item.Id && Items[i].Amount < item.MaxStack)
                {
                    int canAdd = Mathf.Min(item.MaxStack - Items[i].Amount, remaining);
                    Items[i].Amount += canAdd;
                    remaining -= canAdd;

                    if (remaining <= 0)
                        return true;
                }
            }

            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] == null)
                {
                    int toAdd = Mathf.Min(item.MaxStack, remaining);
                    Items[i] = new InventoryItem(item, toAdd);
                    remaining -= toAdd;

                    if (remaining <= 0)
                        return true;
                }
            }
            return false;
        }
        else
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] == null)
                {
                    Items[i] = new InventoryItem(item, 1);
                    return true;
                }
            }
            return false; 
        }
    }

    public void RemoveItemStackAtSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= maxSlots) return;
        Items[slotIndex] = null;
    }

    public bool TryConsume(string itemId, int amount = 1)
    {
        int total = GetTotalAmount(itemId);
        if (total < amount)
            return false;

        int remaining = amount;

        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] != null && Items[i].Item.Id == itemId)
            {
                int take = Mathf.Min(Items[i].Amount, remaining);
                Items[i].Amount -= take;
                remaining -= take;

                if (Items[i].Amount <= 0)
                    Items[i] = null; 

                if (remaining <= 0)
                    break;
            }
        }

        return true;
    }

    public void RemoveItemStack(InventoryItem itemStack)
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == itemStack)
            {
                Items[i] = null;
                break;
            }
        }
    }

    public bool CanAddItem(ItemSO item, int amount = 1)
    {
        if (item.Stackable)
        {
            int remaining = amount;

            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null && Items[i].Item.Id == item.Id)
                {
                    int space = Items[i].Item.MaxStack - Items[i].Amount;
                    remaining -= space;
                    if (remaining <= 0) return true;
                }
            }
        }

        int emptySlots = 0;
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i] == null)
                emptySlots++;
        }

        return emptySlots > 0;
    }

    public int GetTotalAmount(string itemId)
    {
        return Items
            .Where(i => i != null && i.Item.Id == itemId)
            .Sum(i => i.Amount);
    }

    public void GiveStartKit()
    {
        InventoryManager.Instance.Inventory.Items[0] = new InventoryItem(ItemsDatabase.GetItem("0"), 64);
        EventBus.Publish(new InventoryUpdatedEvent());
    }
}