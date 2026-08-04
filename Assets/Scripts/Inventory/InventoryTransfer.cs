using System.Collections;
using System.Collections.Generic;
using FarrokhGames.Inventory;
using UnityEngine;

public class InventoryTransfer : MonoBehaviour
{
    [Header("MainShop")]
    [SerializeField] public GameObject MAINSHOP;
    [SerializeField] public Inventory_Size UtilityInv;
    [SerializeField] public Inventory_Size WeaponInv;
    [SerializeField] public Script_Quest npcQuest;


    [Header("Utility Inventory")]
    [SerializeField] public List<int> inventoryUtilItemsID;
    [SerializeField] public List<Vector2Int> inventoryUtilItemPositions;

    [Header("Weapon Inventory")]
    [SerializeField] public List<int> inventoryWeaponItemsID;
    [SerializeField] public List<Vector2Int> inventoryWeaponItemPositions;

    [Header("Weapon prefab holder")]
    [SerializeField] public List<Script_InvItem> allItemPrefabs;

    private static InventoryTransfer inventoryTransfer;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if(inventoryTransfer == null)
        {
            inventoryTransfer = this;
        }else
        {
            Destroy(gameObject);
        }
    }

    public void ReloadMainShop()
    {
        MAINSHOP.SetActive(true);
        WeaponInv.GetWeaponPrefab();
        UtilityInv.FillInventory();
    }

}
