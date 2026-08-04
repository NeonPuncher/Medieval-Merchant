using System.Collections;
using System.Collections.Generic;
using System.Linq;

using FarrokhGames.Inventory;
using Unity.VisualScripting;
using UnityEditor.VersionControl;

using UnityEngine;

public class Inventory_Size : MonoBehaviour
{
    [SerializeField] private InventoryRenderMode renderMode = InventoryRenderMode.Grid;
    [SerializeField] public int maximumAlowedItemCount = -1;
    [SerializeField] private Inventory_ItemType allowedItem = Inventory_ItemType.Any;
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 4;
    [SerializeField] public List<Script_InvItem> allItems = null;
    [SerializeField] private bool fillRandomly = true; // Should the inventory get filled with random items?
    [SerializeField] private bool fillSave = true;

    public InventoryManager inventory;

    void Start()
    {
        var controller = GameObject.FindAnyObjectByType<InventoryController>();
        var provider = new Inventory_Provider(renderMode, maximumAlowedItemCount, allowedItem);

        //Creating Inventory
        inventory = new InventoryManager(provider, height, height);


        ClearAllItems();

        //REACTIVATE THIS!!!!!
        GetWeaponPrefab();

        //Filling Inventory with random items
        if (fillRandomly)
        {
            FillRandomly();
        }

        //If item is moved or anything, SAVE inventory
        if (fillSave == true)
        {
            FillInventory();
        }

        //Trigger inventory draw system
        GetComponent<InventoryRenderer>().SetInventory(inventory, provider.inventoryRenderMode);
    }

    public void FillRandomly()
    {
        var tries = (width * height) / 2;
        for (var i = 0; i < tries; i++)
        {
            inventory.TryAdd(allItems[Random.Range(0, allItems.Count)].CreateInstance());
        }
        if(inventory.allItems.Length == 0)
        {
            FillRandomly();
        }
    }

    public void ClearAllItems()
    {
        inventory.Clear();
    }

    public void SaveInventory()
    {
        //Clear lists
        InventoryTransfer transfer = FindAnyObjectByType<InventoryTransfer>();

        //Fill utility inventory list
        if (this.GetComponent<Inventory_Size>().allowedItem == Inventory_ItemType.Utility)
        {
            transfer.inventoryUtilItemsID.Clear();
            transfer.inventoryUtilItemPositions.Clear();
            if (this.GetComponent<Inventory_Size>().inventory != null)
            {
                foreach (var item in this.GetComponent<Inventory_Size>().inventory.allItems)
                {
                    //SAVE THE ITEMS IN A FILE WITH THEIR POSITION, THIS CAN BE USED TO REFILL IN THE INVENTORY
                    transfer.inventoryUtilItemsID.Add(item.ID);
                    transfer.inventoryUtilItemPositions.Add(item.position);
                }
            }

        }

        //Fill weapon inventory list
        if (this.GetComponent<Inventory_Size>().allowedItem == Inventory_ItemType.Weapon)
        {
            transfer.inventoryWeaponItemsID.Clear();
            transfer.inventoryWeaponItemPositions.Clear();
            if (this.GetComponent<Inventory_Size>().inventory != null)
            {
                foreach (var item in this.GetComponent<Inventory_Size>().inventory.allItems)
                {
                    //SAVE THE ITEMS IN A FILE WITH THEIR POSITION, THIS CAN BE USED TO REFILL IN THE INVENTORY
                    transfer.inventoryWeaponItemsID.Add(item.ID);
                    transfer.inventoryWeaponItemPositions.Add(item.position);
                }
            }

        }
    }

    //Saves currently held items in inventory
    public void SaveInventoryOnMove(IInventoryItem obj)
    {
        SaveInventory();
    }

    //Repopulate the inventory from save list
    public void FillInventory()
    {
        if (allowedItem == Inventory_ItemType.Utility && FindAnyObjectByType<InventoryTransfer>().inventoryUtilItemsID.Count > 0)
        {
            //CLEAR ENTIRE INVENTORY BEFORE REPOPULATING
            inventory.Clear();

            //REPOPULATE INVENTORY WITH SAVED ITEMS
            for (var i = 0; i < FindAnyObjectByType<InventoryTransfer>().inventoryUtilItemsID.Count; i++)
            {
                inventory.TryAddAt(allItems[FindAnyObjectByType<InventoryTransfer>().inventoryUtilItemsID[i]].CreateInstance(), FindAnyObjectByType<InventoryTransfer>().inventoryUtilItemPositions[i]);
            }
        }

        if (allowedItem == Inventory_ItemType.Weapon && FindAnyObjectByType<InventoryTransfer>().inventoryWeaponItemsID.Count > 0 && inventory != null)
        {
            //CLEAR ENTIRE INVENTORY BEFORE REPOPULATING
            //inventory.Clear();

            for (var i = 0; i < FindAnyObjectByType<InventoryTransfer>().inventoryWeaponItemsID.Count; i++)
            {
                inventory.TryAddAt(allItems[FindAnyObjectByType<InventoryTransfer>().inventoryWeaponItemsID[i]].CreateInstance(), FindAnyObjectByType<InventoryTransfer>().inventoryWeaponItemPositions[i]);
            }
        }
    }

    //Get Weapon Prefabs through lists
    public void GetWeaponPrefab()
    {
        //if (allowedItem == Inventory_ItemType.Weapon)
        //{
        //    InventoryTransfer transfer = FindAnyObjectByType<InventoryTransfer>();
        //    allItems.Clear();
        //    foreach (Script_InvItem item in transfer.allItemPrefabs)
        //    {
        //        allItems.Add(item);
        //    }
        //}
    }
}
