using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using FarrokhGames.Inventory;

[CreateAssetMenu(fileName = "Data", menuName = "MedievalMerchant/Recipe", order = 1)]
public class Script_Recipe : ScriptableObject
{
    //List for all required materials
    public List<string> recipeTypes;
    
    //List of materials for specific key combination in the recipe
    //Usually the part with the most objects attached 
    public List<string> recipeKeyTypes;

    //List of properties inherent for the finished item
    [Header("Inventory Variables")]

    [SerializeField] public Sprite itemSprite = null;
    [SerializeField] public int itemID;
    [SerializeField] public InventoryShape shape = null;
    [SerializeField] public string ItemTemplate;
    [SerializeField] public Inventory_ItemType type = Inventory_ItemType.Utility;
}
