using System.Collections;
using System.Collections.Generic;

using FarrokhGames.Inventory;

using UnityEngine;

using static UnityEditor.Progress;

public class Inventory_Provider : IInventoryProvider
{
    private List<IInventoryItem> items = new List<IInventoryItem>();
    private int _maximumAllowedItemCount;
    Inventory_ItemType _allowedItem;

    public Inventory_Provider(InventoryRenderMode renderMode, int maximumAllowedItemCount = -1, Inventory_ItemType allowedItem = Inventory_ItemType.Any)
    {
        inventoryRenderMode = renderMode;
        _maximumAllowedItemCount = maximumAllowedItemCount;
        _allowedItem = allowedItem;
    }

    public int inventoryItemCount => items.Count;
    public InventoryRenderMode inventoryRenderMode { get; private set; }

    public bool isInventoryFull
    {
        get
        {
            if (_maximumAllowedItemCount < 0) return false;
            return inventoryItemCount >= _maximumAllowedItemCount;
        }
    }

    public bool AddInventoryItem(IInventoryItem item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            return true;
        }
        return false;
    }

    public bool DropInventoryItem(IInventoryItem item)
    {
        return RemoveInventoryItem(item);
    }

    public IInventoryItem GetInventoryItem(int index)
    {
        return items[index];
    }

    public bool CanAddInventoryItem(IInventoryItem item)
    {
        if (_allowedItem == Inventory_ItemType.Any) return true;
        return (item as Script_InvItem).Type == _allowedItem;
    }

    public bool CanRemoveInventoryItem(IInventoryItem item)
    {
        return true;
    }

    public bool CanDropInventoryItem(IInventoryItem item)
    {
        return true;
    }

    public bool RemoveInventoryItem(IInventoryItem item)
    {
        return items.Remove(item);
    }
}
