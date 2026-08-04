using UnityEngine;
using TMPro;
using FarrokhGames.Inventory;
using Unity.VisualScripting;

public class Inventory_Selection : MonoBehaviour
{
    public bool isVendor;
    public string vendorItem;

    void Start()
    {
        var thisController = this.GetComponent<InventoryController>();
        thisController.onItemHovered += AddedItem;
    }

    private void AddedItem(IInventoryItem item)
    {
        if (item != null)
        {
            Debug.Log((item as Script_InvItem).Name);
            if (isVendor == true)
            {
                vendorItem = (item as Script_InvItem).itemTemplate;
            }
        }
    }
}
