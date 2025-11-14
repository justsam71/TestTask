using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ItemsDatabase
{
    public static Dictionary<string, ItemSO> Items = new();

    public static void Initialize()
    {
        var allItems = Resources.LoadAll<ItemSO>("Items");
        foreach (var item in allItems)
            Items[item.Id] = item;
    }

    public static ItemSO GetItem(string id) => Items[id];
}
