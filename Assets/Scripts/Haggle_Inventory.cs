using FarrokhGames.Inventory;

using UnityEngine;

public class Haggle_Inventory : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InventoryManager inventory;
    public Haggle_System system;
    enum InventoryType { crafting, haggling, potion, enchanting }
    [SerializeField] InventoryType inventorytype;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        system = FindAnyObjectByType<Haggle_System>();
        var thisController = this.GetComponent<InventoryController>();
        thisController.onItemDropped += AddCraftItem;
        thisController.onItemReturned += RemoveCraftItem;
        inventory = FindAnyObjectByType<Inventory_Size>().inventory;
    }

    private void AddCraftItem(IInventoryItem item)
    {
        system.SetShape(item as Script_InvItem);
    }

    private void RemoveCraftItem(IInventoryItem item)
    {
        inventory.TryRemove(item);
    }

    public void ReturnItem(IInventoryItem item)
    {
        Debug.Log(item);
        Debug.Log(inventory);
        inventory.TryAdd(item);
    }
}
