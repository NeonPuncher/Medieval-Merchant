using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using TMPro;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class Crafting_System : MonoBehaviour
{

    public const float CellSize = 1f;

    [SerializeField] private Crafting_Preview previewPrefab;
    [SerializeField] private Crafting_Item itemPrefab;
    [SerializeField] private Crafting_Grid grid;

    [SerializeField] private List<Crafting_Data> shapes;
    [SerializeField] private Crafting_Inventory inventory;
    [SerializeField] private Inventory_Size weaponInventory;

    private Crafting_Preview preview;
    public Crafting_Item selectedItem;
    public List<Vector3> SelectedPlacePositions;
    private List<int> matType;
    private int selectedShape;

    public List<Script_CraftRecipe> availableRecipes;

    public int weaponValue;
    public Script_CraftRecipe completeWeaponRecipe;

    public Image weaponSprite;
    public TMP_InputField weaponNameText;
    public TextMeshProUGUI weaponTypeText;
    public TextMeshProUGUI weaponValueText;
    public TextMeshProUGUI weaponAttributeText;

    private void Start()
    {
        matType = null;
    }


    private void Update()
    {
        Vector3 mousePos = GetMouseWorldPosition();

        if(preview != null )
        {
            HandlePreview(mousePos);
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                GetItem();
                if(selectedItem != null)
                {
                    //Pickup item
                    PickupItem();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                GetItem();
                if (selectedItem != null)
                {
                    //Return item to inventory
                    RemoveItem();
                }
            }
        }
    }

    public void SetShape(Script_InvItem item)
    {
        selectedShape = item.gridShape;
        Vector3 mousePos = GetMouseWorldPosition();
        if(preview != null )
        {
            Destroy(preview.gameObject);
        }
        preview = CreatePreview(shapes[selectedShape], mousePos, item);
    }

    public void CheckRecipeProxy()
    {
        foreach(Script_CraftRecipe recipe in availableRecipes)
        {
            grid.highestPerc = 0;
            grid.recipeScore = 0;
            grid.CheckRecipe(recipe);
        }
    }

    private void HandlePreview(Vector3 mouseWorldPosition)
    {
        preview.transform.position = mouseWorldPosition;
        List<Vector3> placePositions = preview.CraftingModel.GetAllCraftingPositions();
        bool canPlace = grid.CanPlace(placePositions);
        if(canPlace)
        {
            preview.transform.position = GetSnappedCenterPosition(placePositions);
            preview.ChangeState(Crafting_Preview.CraftingPreviewState.POSITIVE);
            if (Input.GetMouseButtonDown(0)) 
            {
                PlaceItem(placePositions);
            }
        }
        else
        {
            preview.ChangeState(Crafting_Preview.CraftingPreviewState.NEGATIVE);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            preview.Rotate(90);
        }
    }

    //Place the item with the correct inventory data at the desired coordinates
    private void PlaceItem(List<Vector3> placePositions)
    {
        Crafting_Item craftItem = Instantiate(itemPrefab, preview.transform.position, Quaternion.identity);
        matType = preview.Data.itemMat;
        craftItem.Setup(preview.Data, preview.CraftingModel.Rotation, matType);

        grid.SetCrafting(craftItem, placePositions);

        grid.GetAllOccupied();
        Destroy(preview.gameObject);
        preview = null;
    }

    private void RemoveItem()
    {
        //Destroy Item
        inventory.ReturnItem(selectedItem.data);
        grid.RemoveCrafting(SelectedPlacePositions);
        Destroy(selectedItem.gameObject);
    }

    private void PickupItem()
    {
        grid.RemoveCrafting(SelectedPlacePositions);
        SetShape(selectedItem.data);
        Destroy(selectedItem.gameObject);
    }

    public void ResetCraft()
    {
        Crafting_Item[] items = FindObjectsByType<Crafting_Item>(FindObjectsSortMode.None);
        foreach (Crafting_Item item in items)
        {
            Destroy(item.gameObject);
        }
    }

    private Vector3 GetSnappedCenterPosition(List<Vector3> allCraftingPositions)
    {
        List<int> xs = allCraftingPositions.Select(p => Mathf.FloorToInt(p.x)).ToList();
        List<int> zs = allCraftingPositions.Select(p => Mathf.FloorToInt(p.z)).ToList();
        float centerX = (xs.Min() + xs.Max()) / 2f + CellSize / 2f;
        float centerZ = (zs.Min() + zs.Max()) / 2f + CellSize / 2f;
        return new Vector3(centerX, 0, centerZ);

    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new(Vector3.up, Vector3.zero);

        if(groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    private Crafting_Preview CreatePreview(Crafting_Data data, Vector3 position, Script_InvItem item)
    {
        Crafting_Preview craftingPreview = Instantiate(previewPrefab, position, Quaternion.identity);
        craftingPreview.Setup(item);
        return craftingPreview;
    }

    private void GetItem()
    {
        //Get selected item through Ray casting
        SelectedPlacePositions.Clear();
        selectedItem = null;
        RaycastHit raycastHitObject;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out raycastHitObject, 100f))
        {
            if (raycastHitObject.transform != null)
            {
                //This is your item you clicked sir
                if (raycastHitObject.transform.parent.parent.parent.GetComponent<Crafting_Item>() != null)
                {
                    selectedItem = raycastHitObject.transform.parent.parent.parent.GetComponent<Crafting_Item>();
                    foreach(Transform child in raycastHitObject.transform.parent)
                    {
                        if(child.GetComponent<Crafting_ShapeUnit>() != null)
                        {
                            SelectedPlacePositions.Add(child.transform.position);
                        }
                    }
                    selectedShape = raycastHitObject.transform.parent.parent.parent.GetComponent<Crafting_Item>().data.gridShape;
                    matType = raycastHitObject.transform.parent.parent.parent.GetComponent<Crafting_Item>().data.itemMat;

                }
            }
        }
    }
    
    //Check recipe and if valid open weapon info screen 
    public void ShowWeaponInfo()
    {
        completeWeaponRecipe = null;
        CheckRecipeProxy();
        if(completeWeaponRecipe != null)
        {
            weaponSprite.sprite = completeWeaponRecipe.weaponTemplate.itemSprite;
            weaponTypeText.text = completeWeaponRecipe.recipeName;
            weaponValue = completeWeaponRecipe.weaponTemplate.value;
            weaponValueText.text = weaponValue.ToString();
        }
    }

    //Finish weapon and add it to the weapon inventory
    public void FinishWeapon()
    {
        Script_InvItem newWeapon = Instantiate(completeWeaponRecipe.weaponTemplate);
        newWeapon.name = weaponNameText.text;
        newWeapon.itemName = weaponNameText.text;
        newWeapon.value = weaponValue;
        weaponInventory.allItems.Add(newWeapon);
        weaponInventory.inventory.TryAdd(newWeapon);
    }
}
