using System.Collections;
using System.Collections.Generic;
using FarrokhGames.Inventory;
using UnityEngine;

[CreateAssetMenu(fileName = "InvItem", menuName = "MedievalMerchant/InvItem", order = 1)]
public class Script_InvItem : ScriptableObject, IInventoryItem
{
    [Header("Inventory Variables")]

    [SerializeField] public Sprite itemSprite = null;
    [SerializeField] public int itemID;
    [SerializeField] public string itemTemplate;
    [SerializeField] public InventoryShape shape = null;
    [SerializeField] public int value;
    [SerializeField] public Inventory_ItemType type = Inventory_ItemType.Utility;
    [SerializeField] private bool itemCanDrop = false;
    [SerializeField, HideInInspector] private Vector2Int itemPosition = Vector2Int.zero;

    public string Name => this.name;
    public Inventory_ItemType Type => type;

    public int ID => itemID;
    public Sprite sprite => itemSprite;
    public int width => shape.width;
    public int height => shape.height;
    public Vector2Int position
    {
        get => itemPosition;
        set => itemPosition = value;
    }

    public bool IsPartOfShape(Vector2Int localPosition)
    {
        return shape.IsPartOfShape(localPosition);
    }

    public bool canDrop => itemCanDrop;

    public IInventoryItem CreateInstance()
    {
        var clone = ScriptableObject.Instantiate(this);
        clone.name = clone.name.Substring(0, clone.name.Length - 7);
        return clone;
    }

    [Header("Crafting Variables")]
    public string itemName;
    public int gridShape;
    public List<int> itemMat;

    public enum specialMat
    {
        None, Wood, Fire, Magic, Electricity, Metal, Wet, Breakable, Ability
    };

    //What is this material and with whom will he react?
    public specialMat specialMatSelected;
    public specialMat specialMatReact;

    public GameObject Model;
    public Crafting_Model gridModel;
    public List<Material> Materials;
}
