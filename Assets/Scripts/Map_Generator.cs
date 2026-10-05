using System.Collections;
using System.Collections.Generic;
using System.Text;

using TMPro;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class Map_Generator : MonoBehaviour
{
    [Header("Map Variables")]
    //Generating Variables
    public List<Script_MapTile> mapTiles;
    public GameObject mapTilePrefab;
    public Inventory_Size inventoryVendor;
    public UI_Management uiManager;
    public TextMeshProUGUI fragAmountText;
    public GameObject mapParent;

    private float xcoord;
    private float ycoord;
    private float maxX;
    private float maxY;
    private int thisTile;

    [SerializeField] private int mapWidth, mapHeight;

    public int fragAmount;
    private string mapSeed;

    [Header("Book info variables")]
    public List<Script_Worker> workers;
    private int workerPage;
    private int totalCost;
    public float workTimer;
    public bool sendWorker;
    private bool workerSelected;

    [Header("Book info UI")]
    //UI Panel Variables
    public TextMeshProUGUI tileInfoName;
    public TextMeshProUGUI tileInfoCost;
    public TextMeshProUGUI tileInfoTime;
    public Button sendButton;

    [Header("Worker info UI")]
    public TextMeshProUGUI workerInfoName;
    public TextMeshProUGUI workerInfoCost;
    public Image workerImg;

    //SECRET WORKER STATS
    private int totalFight;
    private int totalSurv;
    private int totalScav;
    private float totalSpeed;

    public GameObject mapUI;
    public GameObject infoBook;
    public GameObject workerParent;
    private int ButtonWorker;
    public GameObject itemParent;
    public GameObject itemPrefab;

    public GameObject workerHUDParent;
    public GameObject workerHUDPrefab;

    //Zooming Variables
    private float currentScale;

    // Start is called before the first frame update
    void Start()
    {
        fragAmountText.text = fragAmount.ToString() + "x map fragments";
        currentScale = Mathf.Round(mapParent.transform.localScale.x);
        //GenMap(mapSeed);
        GenerateBlankMap();
    }

    //GENERATING MAP
    //CENTER MAP
    //GENERATE RANDOM TILE
    //PUT TILE INTO SEED
    void GenerateMap()
    {
        Debug.Log(mapSeed);
        string tempSeed = mapSeed;

        //CLEAR MAP
        foreach (Transform tile in mapParent.transform)
        {
            Destroy(tile.gameObject);
        }

        //GENERATE MAP FROM STRING

        float xOffset = 0;
        float yOffset = 0;

        for (int x = mapWidth; x > 0; x--)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                xOffset = (-(mapWidth / 2) * 20) + ((x * 20) + (y * 20)) / 2f;
                yOffset = ((x * 20) - (y * 20)) / 4f;

                int xCol = mapWidth - x;
                int yCol = y;

                if (mapSeed[0].ToString() != "0")
                {
                    StartCoroutine(GenerateTile(xOffset, yOffset, xCol, yCol));
                }
                else if (mapSeed[0].ToString() == "0")
                {
                    mapSeed = mapSeed.Substring(1);
                }
            }
            mapSeed = mapSeed.Substring(1);
        }

        mapSeed = tempSeed;
    }

    //GENERATE TILE

    IEnumerator GenerateTile(float xOffset, float yOffset, int x, int y)
    {
        foreach (Script_MapTile mapTile in mapTiles)
        {
            //GENERATE TILE
            if (mapSeed[0].ToString() == mapTile.ID)
            {
                GameObject tile = Instantiate(mapTilePrefab);
                tile.transform.parent = mapParent.transform;
                tile.name = mapTile.ID;
                tile.transform.localScale = new Vector3(.2f,.2f,0);
                tile.transform.localPosition = new Vector3(xOffset, yOffset, 0);
                tile.GetComponent<Image>().sprite = mapTile.mapImage;
                tile.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;

                //ADD FUNCTION FOR BUTTON CLICK
                if(mapTile.ID == "e")
                {
                    tile.GetComponent<Button>().onClick.AddListener(delegate { GenerateNewTile(x, y); });
                }
                else
                {
                    tile.GetComponent<Button>().onClick.AddListener(delegate { OnTileClick(mapTile); });
                }


                mapSeed = mapSeed.Substring(1);
                yield break;
            }
        }
    }

    //CREATES THE FIRST BLANK SEED
    void GenerateBlankMap()
    {
        for (int x = mapWidth; x > 0; x--)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                if(x == mapWidth/2 && y == mapHeight/2)
                {
                    mapSeed = mapSeed + "1";
                }
                else
                {
                    mapSeed = mapSeed + "0";
                }
            }
            mapSeed = mapSeed + ",";
        }

        //REPLACE THE ADJACENT TILES OF HOME WITH EMPTY EXPLORABLE TILES

        StringBuilder tempSeed = new StringBuilder(mapSeed);
        tempSeed[(mapWidth * (mapHeight/2)) + (1*mapHeight) + 1] = 'e';
        tempSeed[(mapWidth * (mapHeight / 2)) + (1 * mapHeight) - 1] = 'e';
        tempSeed[(mapWidth * (mapHeight / 2)) + (1 * mapHeight) - (mapWidth + 1)] = 'e';
        tempSeed[(mapWidth * (mapHeight / 2)) + (1 * mapHeight) + (mapWidth + 1)] = 'e';

        mapSeed = tempSeed.ToString();

        GenerateMap();
    }

    //IF TILE IS EMPTY EXPLORABLE, CHECK ADJACENT, IF 0 REPLACE WITH E
    //REDO THIS CODE ONE DAY EVENTUALLY
    void GenerateNewTile(int x, int y)
    {
        //CHECK FOR ADJACENT
        StringBuilder tempSeed = new StringBuilder(mapSeed);
        int index = (mapWidth * x) + y + x;

        //CARDINAL NEIGHBOR OFFSETS FOR UP DOWN LEFT RIGHT
        int[] neighborOffsets = { +1, -1, -mapWidth - 1, mapWidth + 1 };

        foreach (int offset in neighborOffsets)
        {
            int neighborIndex = index + offset;

            //DONT GO OUTSIDE THE BOUNDS
            if (neighborIndex >= 0 && neighborIndex < mapSeed.Length)
            {
                if (mapSeed[neighborIndex] == '0')
                {
                    tempSeed[neighborIndex] = 'e';
                }
            }
        }

        //REPLACE CURRENT TILE AND GEN BASED ON HOW FAR FROM SHOP

        if(Mathf.Abs((mapWidth/2) - x) < 2 || Mathf.Abs((mapHeight / 2) - y) < 2)
        {
            char[] newAreas = { 'v', 'f', 'l', 'h', 'r' };
            tempSeed[index] = newAreas[Random.Range(0, newAreas.Length)];
        }
        else if (Mathf.Abs((mapWidth / 2) - x) < 5 || Mathf.Abs((mapHeight / 2) - y) < 5)
        {
            char[] newAreas = { 's', 'm', 'o', 'k', 'd' };
            tempSeed[index] = newAreas[Random.Range(0, newAreas.Length)];
        }
        else
        {
            char[] newAreas = { 'i', 'b', 'p', 'a' };
            tempSeed[index] = newAreas[Random.Range(0, newAreas.Length)];
        }

        mapSeed = tempSeed.ToString();
        GenerateMap();
    }


    //Generate Map Tile info in Info Book 
    public void OnTileClick(Script_MapTile mapTile)
    {
        totalCost = mapTile.cost;
        workTimer = mapTile.workTime;
        tileInfoName.text = mapTile.mapName;
        tileInfoCost.text = "Costs: " + mapTile.cost.ToString() + " gold";
        tileInfoTime.text = "Time: " + mapTile.workTime.ToString() + " Days";
        sendButton.onClick.RemoveAllListeners();
        sendButton.onClick.AddListener(delegate { SendWorker(mapTile); });
        
        if(infoBook.activeInHierarchy == false)
        {
            infoBook.SetActive(true);
        }

        //CLEAR SELECTED WORKERS AND POPULATE POSSIBLE SUPPLY LIST
        foreach (Transform child in workerParent.transform)
        {
            child.GetComponent<Image>().sprite = workers[0].workerSprite;
        }

        totalFight = 0;
        totalSurv = 0;
        totalScav = 0;
        totalSpeed = 0;

        foreach (Transform child in itemParent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Script_InvItem item in mapTile.rewards)
        {
            //MAKE CHECK THAT ITEM IS ALREADY UNLOCKED
            GameObject itemEntry = Instantiate(itemPrefab, itemParent.transform);
            itemEntry.GetComponent<Image>().sprite = item.sprite;
        }
    }

    //Update the info about the worker
    public void UpdateWorker(int childnum)
    {
        ButtonWorker = childnum;
        workerInfoName.text = "Name: " + workers[workerPage].workerName;
        workerInfoCost.text = "Cost: " + workers[workerPage].workerCost;
        workerImg.sprite = workers[workerPage].workerSprite;
    }

    //Update the page of the info worker
    public void UpdateWorkerPage(int SetPage)
    {
        workerPage = workerPage + SetPage;
        if (workerPage >= 0 && workerPage < workers.Count)
        {
            UpdateWorker(ButtonWorker);
        }
        else
        {
            workerPage = workerPage - SetPage;
        }
    }

    public void ConfirmWorker()
    {
        //Confirm worker and add to total cost
        totalCost = totalCost + workers[workerPage].workerCost;
        tileInfoCost.text = "Costs: " + totalCost.ToString() + " gold";
        workerParent.transform.GetChild(ButtonWorker).GetComponent<Image>().sprite = workers[workerPage].workerSprite;

        //Worker abilities
        totalFight = totalFight + workers[workerPage].workerSkillFight;
        totalSurv = totalSurv + workers[workerPage].workerSkillSurv;
        totalScav = totalScav + workers[workerPage].workerSkillScav;
        totalSpeed = totalSpeed + workers[workerPage].workerSkillSpeed;

        //Reset worker info page
        workerPage = 0;
        if(workerSelected == false)
        {
            workerSelected = true;
        }
    }

    public void SendWorker(Script_MapTile mapTile)
    {
        if(uiManager.money >= totalCost && sendWorker == false && workerSelected == true)
        {
            uiManager.money -= totalCost;

            if (mapTile.workDiff > totalFight)
            {
                //START KILLING WORKERS
            }

            //Scale amount of items found with total scav level
            inventoryVendor.maximumAlowedItemCount = totalScav;

            sendWorker = true;
            inventoryVendor.allItems = mapTile.rewards;
            infoBook.SetActive(false);
            mapUI.SetActive(false);

            //Create worker UI 
            CreateExpedition(mapTile, mapTile.rewards);
        }
        else
        {
            Debug.Log("Not Enough Money");
        }
    }

    private void CreateExpedition(Script_MapTile mapTile, List<Script_InvItem> items)
    {
        GameObject newExpedition = Instantiate(workerHUDPrefab, workerHUDParent.transform);
        Shop_UIWorkerEntry newExpeditionScript = newExpedition.GetComponent<Shop_UIWorkerEntry>();
        newExpeditionScript.availableItems = items;
        newExpeditionScript.expeditionTotal = workTimer;
        newExpeditionScript.expeditionLocationSprite.sprite = mapTile.mapImage;
        for (int i = 0; i < workerParent.transform.childCount; i++)
        {
            newExpeditionScript.expeditionWorkers[i].sprite = workerParent.transform.GetChild(i).GetComponent<Image>().sprite;
        }
    }

    public void Zoom(float scaling)
    {
        currentScale += scaling;
        Debug.Log(currentScale);
        mapParent.transform.localScale = new Vector2(currentScale, currentScale);
    }
}
