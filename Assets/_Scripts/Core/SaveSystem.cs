using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SubscribeToEvents();
    }

    public void SubscribeToEvents()
    {
        EventBus.Subscribe<InventoryUpdatedEvent>(OnInventoryChanged);
    }


    void OnInventoryChanged(InventoryUpdatedEvent e)
    {
        SaveInventory();
    }

    public void SaveInventory()
    {
        SaveData data = new SaveData();
        data.inventory = new List<InventoryItemData>();

        for (int i = 0; i < InventoryManager.Instance.Inventory.Items.Length; i++)
        {
            var item = InventoryManager.Instance.Inventory.Items[i];
            if (item == null) continue;

            data.inventory.Add(new InventoryItemData()
            {
                itemId = item.Item.Id,
                amount = item.Amount,
                slotIndex = i 
            });
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }

    public void LoadInventory()
    {
        string path = Application.persistentDataPath + "/save.json";

        if (!File.Exists(path))
        {
            InventoryManager.Instance.Inventory.GiveStartKit();
            return;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);


        for (int i = 0; i < InventoryManager.Instance.Inventory.Items.Length; i++)
            InventoryManager.Instance.Inventory.Items[i] = null;

        foreach (var entry in data.inventory)
        {
            ItemSO item = ItemsDatabase.GetItem(entry.itemId);
            if (item == null) continue;

            int slot = entry.slotIndex;
            if (slot >= 0 && slot < InventoryManager.Instance.Inventory.Items.Length)
            {
                InventoryManager.Instance.Inventory.Items[slot] = new InventoryItem(item, entry.amount);
            }
        }

        EventBus.Publish(new InventoryUpdatedEvent());
    }
}

[System.Serializable]
public class SaveData
{
    public List<InventoryItemData> inventory;
}

[System.Serializable]
public class InventoryItemData
{
    public string itemId;
    public int amount;
    public int slotIndex;
}