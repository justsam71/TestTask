using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class InventoryItem
{
    public ItemSO Item;
    public int Amount;

    public InventoryItem(ItemSO item, int amount)
    {
        Item = item;
        Amount = amount;
    }
}