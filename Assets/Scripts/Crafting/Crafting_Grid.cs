using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using NUnit.Framework;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;

using static UnityEditor.Progress;

public class Crafting_Grid : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    public CraftingGridCell[,] grid;
    private Crafting_System system;

    private int totalMats;

    //Recipe check variables
    private int lowestY;
    private string rowString;
    private string proxyGridFill;
    public string recipeCheck;
    private float simPerc;
    public float highestPerc;
    private string mostSimilar;
    public int recipeScore;
    public int itemCount;
    public int totalValue;

    public bool isHaggle;

    private void Start()
    {
        system = FindAnyObjectByType<Crafting_System>();
        totalValue = 0;
        //Create field of CraftingGrid Cells
        grid = new CraftingGridCell[width, height];
        itemCount = 0;
        lowestY = height;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = new();
                grid[x, y].x = x;
                grid[x, y].y = y;
            }
        }
    }

    public void SetCrafting(Crafting_Item craftItem, List<Vector3> allCraftingPositions)
    {
        //Cycle through all mats defined in data list
        totalMats = 0;
        itemCount++;

        //For all positions that will be occupied
        foreach (var p in allCraftingPositions)
        {
            (int x, int y) = WorldToGridPosition(p);
            grid[x, y].SetCrafting(craftItem, totalMats, itemCount);
            totalMats++;
        }
    }

    public void RemoveCrafting(List<Vector3> allCraftingPositions)
    {
        foreach(var p in allCraftingPositions)
        {
            (int x, int y) = WorldToGridPosition(p);
            grid[x, y].IsEmpty();
        }
        itemCount--;
    }

    //Get Full grid with all materials, 0 = empty 
    public void GetAllOccupied()
    {
        totalValue = 0;
        foreach(CraftingGridCell cell in grid)
        {
            if (cell.IsEmpty()) 
            {

            }
            else
            {
                totalValue = totalValue + cell.value;

                if (cell.y < lowestY)
                {
                    lowestY = cell.y;
                    Debug.Log(lowestY);
                }
            }
        }
    }

    public void CheckRecipe(Script_CraftRecipe recipe)
    {
        GetAllOccupied();
        //Check if the Y space available fits the recipe, if not, return
        if (recipe.gridSizeY - lowestY >= 0)
        {
            simPerc = 0;
            recipeCheck = "";
            for (int l = 0; l <= width - recipe.gridSizeX; l++)
            {
                rowString = "";
                proxyGridFill = "";
                //Give this function the scriptable object with recipe
                for (int y = lowestY; y < recipe.gridSizeY; y++)
                {
                    //Start at the right y coord by checking the lowest point occupied
                    for (int x = 0 + l; x < recipe.gridSizeX + l; x++)
                    {
                        //Add material to string if present otherwise add x
                        if (!grid[x,y].IsEmpty())
                        {
                            Debug.Log("try " + l + "grid: " + grid[x, y].mat);
                            rowString += grid[x, y].mat;
                        }
                        else
                        {
                            rowString += "x";
                        }
                    }
                }
                Debug.Log(rowString);
                //Check if recipe and grid have equal amount of material and displacement
                if(rowString.Length == recipe.recipeRawKey.Length)
                {
                    for (int i = 0; i < recipe.recipeRawKey.Length; i++)
                    {
                        //Remove the empty spaces
                        if (recipe.recipeRawKey[i] != 'x')
                        {
                            proxyGridFill += rowString[i];
                        }
                    }
                }

                //Check how similar the gridfill is to the true key
                //Only for recipes that are longer
                if (proxyGridFill.Length >= recipeScore)
                {
                    simPerc = CompareRecipe(proxyGridFill, recipe.recipeKey);
                    if (simPerc >= 80 && simPerc >= highestPerc)
                    {
                        Debug.Log("Similarity: " + simPerc);
                        Debug.Log("Completed the " + recipe.recipeName);
                        recipeScore = proxyGridFill.Length;
                        highestPerc = simPerc;
                        mostSimilar = proxyGridFill;
                        system.completeWeaponRecipe = recipe;
                        return;
                    }
                }
            }
        }
        else
        {
            return;
        }
        
    }

    //Check recipe similarity to gridfill within recipe bounds
    private float CompareRecipe(string inputGrid, string inputRecipe)
    {
        float similarPerc;
        float recipePerc;

        similarPerc = 0;
        recipePerc = 100 / inputRecipe.Length;

        Debug.Log(inputGrid);
        Debug.Log(inputRecipe);

        for (int i = 0; i < inputGrid.Length; i++)
        {
            if (inputGrid[i] == inputRecipe[i])
            {
                similarPerc += recipePerc;
            }
        }
        return similarPerc;
    }


    //Check recipe similarity to gridfill outside recipe bounds
    public void FindEmpty()
    {
        Crafting_Item[] items;


        items = FindObjectsByType<Crafting_Item>(FindObjectsSortMode.None);

        if(items.Length > 0)
        {
            foreach (Crafting_Item item in items)
            {
                IsConnected(item);
            }
        }
        else
        {
            Debug.Log("No items on grid");
        }
    }

    //Checks for connected pieces to all crafting items
    //Further checks for specialised connections
    private void IsConnected(Crafting_Item item)
    {
        Crafting_ShapeUnit[] shapeCoords;
        List<Vector3> neighbours;
        string itemId;
        itemId = "";

        itemId = item.itemName;
        neighbours = new List<Vector3>();
        shapeCoords = item.GetComponentsInChildren<Crafting_ShapeUnit>();
        //Find coords that shape is using
        foreach (Crafting_ShapeUnit shapeUnit in shapeCoords)
        {
            //Get all neighbour coords
            neighbours.Add(new Vector3((shapeUnit.transform.position.x - Crafting_System.CellSize), shapeUnit.transform.position.y, shapeUnit.transform.position.z));
            neighbours.Add(new Vector3((shapeUnit.transform.position.x + Crafting_System.CellSize), shapeUnit.transform.position.y, shapeUnit.transform.position.z));
            neighbours.Add(new Vector3(shapeUnit.transform.position.x, shapeUnit.transform.position.y, (shapeUnit.transform.position.z + Crafting_System.CellSize)));
            neighbours.Add(new Vector3(shapeUnit.transform.position.x, shapeUnit.transform.position.y, (shapeUnit.transform.position.z - Crafting_System.CellSize)));
        }
        //Remove duplicates
        neighbours = neighbours.Distinct().ToList();
        //Check if neighbour has tile that is not empty or itself
        foreach (Vector3 v in neighbours)
        {
            Debug.Log(v);
            if(v.x >= -width/2 && v.x <= width/2 && v.z >= -height/2 && v.z <= height/2)
            {
                //Convert vector 3 to grid coords
                (int x, int y) = WorldToGridPosition(v);
                if (!grid[x, y].IsEmpty() && grid[x, y].itemName != itemId)
                {
                    //CAN MAKE SPECIAL CONNECTION INTERACTIONS!?!?
                    if (grid[x,y].specialMat == item.data.specialMatReact.ToString())
                    {
                        Debug.Log("EXPLOSION!!!!");
                    }

                    return;
                }
                else
                {
                    Debug.Log("This piece is not connected");
                    //Cannot finish build
                }
            }

        }
    }

    public bool CanPlace(List<Vector3> allCraftingPositions)
    {
        foreach (var p in allCraftingPositions)
        {
            (int x, int y) = WorldToGridPosition(p);
            if(x<0|| x>= width || y<0 || y>= height) return false;
            if (!grid[x,y].IsEmpty()) return false;
        }
        return true;
    }

    private (int x, int y) WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - transform.position).x / Crafting_System.CellSize);
        int y = Mathf.FloorToInt((worldPosition - transform.position).z / Crafting_System.CellSize);
        return (x, y);
    }

    private void OnDrawGizmos()
    {
        //Draw grid
        Gizmos.color = Color.yellow;
        if (Crafting_System.CellSize <= 0 || width <= 0 || height <= 0) return;
        Vector3 origin = transform.position;
        for(int y = 0; y <= height; y++)
        {
            Vector3 start = origin + new Vector3 (0, 0.01f, y * Crafting_System.CellSize);
            Vector3 end = origin + new Vector3(width * Crafting_System.CellSize, 0.01f, y * Crafting_System.CellSize);
            Gizmos.DrawLine(start, end);
        }
        for (int x = 0; x <= width; x++)
        {
            Vector3 start = origin + new Vector3(x * Crafting_System.CellSize, 0.01f, 0);
            Vector3 end = origin + new Vector3(x * Crafting_System.CellSize, 0.01f, height * Crafting_System.CellSize);
            Gizmos.DrawLine(start, end);
        }
    }
}

//Create the individual cells for the grid and set all their variables
public class CraftingGridCell
{
    private Crafting_Item craftItem;
    public int mat;
    public string itemName;
    public string specialMat;
    public string specialReact;
    public int shape;
    public int value;
    public int x;
    public int y;

    public void SetCrafting(Crafting_Item craftItem, int currentMat, int itemCounter)
    {
        this.craftItem = craftItem;
        this.specialMat = craftItem.data.specialMatSelected.ToString();
        this.specialReact = craftItem.data.specialMatReact.ToString();
        this.itemName = craftItem.data.name + itemCounter;
        this.value = craftItem.data.value;
        this.craftItem.itemName = craftItem.data.name + itemCounter;
        for (int i = 0; i < craftItem.data.itemMat.Count; i++)
        {
            this.mat = craftItem.data.itemMat[currentMat];
            this.shape = craftItem.data.gridShape;
        }
    }

    public bool IsEmpty()
    {
        return craftItem == null;
    }
}
