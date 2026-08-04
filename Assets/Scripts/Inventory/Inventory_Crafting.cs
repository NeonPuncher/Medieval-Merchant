using System.Collections;
using System.Collections.Generic;

using FarrokhGames.Inventory;

using UnityEngine;

using static UnityEditor.Progress;

public class Inventory_Crafting : MonoBehaviour
{
    public InventoryManager inventory;
    public GameObject itemPrefab;
    public string currentItem;

    [SerializeField] private RectTransform imgRectTransform;

    enum InventoryType {crafting,haggling,potion,enchanting}
    [SerializeField] InventoryType inventorytype;

    private Inventory_Size inventorySize;
    private Vector3 mousePosition;
    private Vector3 RandomLocation;

    void Start()
    {
        var thisController = this.GetComponent<InventoryController>();
        thisController.onItemDropped += AddCraftItem;
        thisController.onItemReturned += RemoveCraftItem;
        inventory = FindAnyObjectByType<Inventory_Size>().inventory;
    }

    private void AddCraftItem(IInventoryItem item)
    {
        Debug.Log(item.ToString());
        Vector2 localMousePosition = imgRectTransform.InverseTransformPoint(Input.mousePosition);
        if(imgRectTransform.rect.Contains(localMousePosition))
        {
            Debug.Log("Mouse is inside bounds");
            SpawnItem(item as Script_InvItem);
        }
        else
        {
            Debug.Log("Mouse outside bounds");
            inventory.TryAdd(item);
        }
    }

    private void RemoveCraftItem(IInventoryItem item)
    {
        inventory.TryRemove(item);
    }

    public void ReturnItem(IInventoryItem item)
    {
        Debug.Log(item);
        inventory.TryAdd(item);
    }

    private void SpawnItem(Script_InvItem item)
    {
        // Get mouse position in screen space
        Vector3 mousePos = Input.mousePosition;

        // Determine the distance from the camera to the ground plane
        // (You can tune this if your camera setup changes)
        float depth = Camera.main.WorldToScreenPoint(Vector3.zero).z + 0f;
        float spawnHeight = 2f;

        // Convert mouse position to world space
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));

        // Keep only X and Z from mouse, apply adjustable spawnHeight for Y
        Vector3 spawnPos = new Vector3(worldPos.x, spawnHeight, worldPos.z);

        // Spawn the item with a rotation facing upward
        GameObject itemClone = Instantiate(itemPrefab, spawnPos, Quaternion.Euler(90, 0, 0));

        if (inventorytype == InventoryType.crafting)
        {
            //itemClone.GetComponent<Craft_Item>().itemInfo = item;
            //itemClone.name = item.Name + FindAnyObjectByType<Craft_ItemManager>().totalitems.ToString();
            //FindAnyObjectByType<Craft_ItemManager>().totalitems++;
        }

        if (inventorytype == InventoryType.haggling)
        {
            //itemClone.GetComponent<Haggle_Item>().itemInfo = item;
            //itemClone.name = item.Name + FindAnyObjectByType<Haggle_ItemManager>().totalitems.ToString();
            //FindAnyObjectByType<Haggle_ItemManager>().totalitems++;
        }
    }
}
