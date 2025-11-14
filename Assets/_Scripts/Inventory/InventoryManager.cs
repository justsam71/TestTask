using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int maxSlots = 4;
    public static InventoryManager Instance;

    public Inventory Inventory { get; private set; }

    private void Awake()
    {
        Instance = this;
        Inventory = new Inventory(maxSlots);
        ItemsDatabase.Initialize();
    }

    private void Start()
    {
        SaveSystem.Instance.LoadInventory();
    }

    public void AddItem(ItemSO item, int amount)
    {
        Inventory.AddItem(item, amount);
        EventBus.Publish(new InventoryUpdatedEvent());
    }


    public void RemoveAllItemsFromSlot(int slotIndex)
    {
        Inventory.RemoveItemStackAtSlot(slotIndex);
        EventBus.Publish(new InventoryUpdatedEvent());
    }

    public int GetAmmoCount(string ammoId)
    {
        return Inventory.GetTotalAmount(ammoId);
    }
}
