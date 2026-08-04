using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UIElements;

using static UnityEditor.Progress;

public class Haggle_System : MonoBehaviour
{
    public const float CellSize = 1f;

    [SerializeField] private Crafting_Preview previewPrefab;
    [SerializeField] private Crafting_Item itemPrefab;
    [SerializeField] private Crafting_Grid grid;

    [SerializeField] private List<Crafting_Data> shapes;
    [SerializeField] private Crafting_Inventory inventory;
    [SerializeField] private Inventory_Size weaponInventory;

    [SerializeField] private List<Script_HaggleAbility> NPCMoves;
    [SerializeField] private List<Script_HaggleAbility> playerMoves;

    private Crafting_Preview preview;
    public Crafting_Item selectedItem;
    public List<Vector3> SelectedPlacePositions;
    private List<int> matType;
    private int selectedShape;
    private bool NPCTurn;

    public TextMeshProUGUI moneyText;

    private void Start()
    {
        matType = null;
    }


    private void Update()
    {
        Vector3 mousePos = GetMouseWorldPosition();

        if (preview != null)
        {
            HandlePreview(mousePos);
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                GetItem();
                if (selectedItem != null)
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
        {
            selectedShape = item.gridShape;
            Vector3 mousePos = GetMouseWorldPosition();
            if (preview != null)
            {
                Destroy(preview.gameObject);
            }
            preview = CreatePreview(shapes[selectedShape], mousePos, item);
        }
    }

    //Floating preview, check if the preview on the mouse position can be place or not, rotate the object 
    private void HandlePreview(Vector3 mouseWorldPosition)
    {
        preview.transform.position = mouseWorldPosition;
        List<Vector3> placePositions = preview.CraftingModel.GetAllCraftingPositions();
        bool canPlace = grid.CanPlace(placePositions);
        if (canPlace)
        {
            preview.transform.position = GetSnappedCenterPosition(placePositions);
            preview.ChangeState(Crafting_Preview.CraftingPreviewState.POSITIVE);
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(mouseWorldPosition);
                PlaceItem(placePositions);
                NPCTurn = true;
                NPCMove();
            }
        }
        else
        {
            preview.ChangeState(Crafting_Preview.CraftingPreviewState.NEGATIVE);
        }
        if (Input.GetKeyDown(KeyCode.R))
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
        moneyText.text = grid.totalValue.ToString();
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

        if (groundPlane.Raycast(ray, out float distance))
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

    //Cast ray from mouse position and check if it hits an object
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
                    foreach (Transform child in raycastHitObject.transform.parent)
                    {
                        if (child.GetComponent<Crafting_ShapeUnit>() != null)
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

    private void NPCMove()
    {
        Debug.Log("NPC turn");
        //Know which positions are empty from getting the grid
        //Make a list of all potential moves
        //Check which ability has the most effect on the score in favour of the NPC (Chess bots)
        //FUTURE TYPE SHIT

        //Random Position on empty grid with shape and rotation
        //Change that to mousePosition
        //Check with canPlace / preview on that position

        foreach (Script_HaggleAbility currentAbility in NPCMoves)
        {
            foreach (CraftingGridCell cell in grid.grid)
            {
                if (cell.IsEmpty())
                {
                    Debug.Log(cell.x + " " + cell.y);
                    Debug.Log(GridToWorldPosition(cell.x, cell.y));

                    Vector3 NPCPosition = GridToWorldPosition(cell.x, cell.y);
                    Crafting_Preview NPCPreview = Instantiate(previewPrefab, NPCPosition, Quaternion.identity);
                    NPCPreview.CraftingModel = currentAbility.gridModel;
                    List<Vector3> NPCplacePositions = NPCPreview.CraftingModel.GetAllCraftingPositions();
                    bool canPlace = grid.CanPlace(NPCplacePositions);

                    Debug.Log(NPCplacePositions);

                    if (canPlace)
                    {
                        NPCPreview.transform.position = GetSnappedCenterPosition(NPCplacePositions);
                        Debug.Log("Place item");
                        Crafting_Item abilityItem = Instantiate(itemPrefab, NPCPreview.transform.position, Quaternion.identity);
                        abilityItem.SetUpAbility(currentAbility, 0, matType);
                        matType = currentAbility.itemMat;
                        grid.SetCrafting(abilityItem, NPCplacePositions);

                        grid.GetAllOccupied();
                        Destroy(NPCPreview.gameObject);
                        preview = null;
                        NPCTurn = false;
                        return;
                    }

                    else
                    {
                        Debug.Log("Too slow");
                        break;
                    }
                }
            }
        }
    }

    private Vector3 GridToWorldPosition(int x, int y)
    {

        float halfCell = CellSize * 0.5f;

        return transform.position + new Vector3(
            x * CellSize + halfCell,
            0f,
            y * CellSize + halfCell
        );
    }
}
