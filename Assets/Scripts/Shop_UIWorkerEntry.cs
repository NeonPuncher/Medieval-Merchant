using System.Collections.Generic;

using Ink.Parsed;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class Shop_UIWorkerEntry : MonoBehaviour
{

    public Image expeditionLocationSprite;
    public float expeditionTotal;
    private float expeditionProgress;
    public List<Image> expeditionWorkers;
    public Slider expeditionProgressSlider;
    public List<Script_InvItem> availableItems;
    public List<Script_InvItem> cartItems;

    [SerializeField] private Inventory_Size cartInventory;
    private int totalWeight;

    void Start()
    {
        expeditionProgressSlider.value = 0;
        expeditionProgress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        expeditionProgress += Time.deltaTime;

        expeditionProgressSlider.value = (1/expeditionTotal) * expeditionProgress;

        if(expeditionProgressSlider.value >= 1)
        {
            //Destroy(this.gameObject);
        }
    }

    private void MakeLootTable()
    {
        //Get all items that can be given
        //Check their rarity
        //Have a random item be chosen based on rarity -> rare items less common to be chosen

        foreach (var item in availableItems)
        {
            totalWeight += item.quality;
        }

        int averageQualityConversion = 50 + ();

        for(int i = 0; i < cartInventory.maximumAlowedItemCount; i++)
        {

            int qualityRandom = Random.Range(0, averageQualityConversion);
            if (qualityRandom <= 50)
            {
                //Rarity 1
            }
            if (qualityRandom <= 75 && qualityRandom >= 51)
            {
                //Rarity 2
            }
            if (qualityRandom <= 88 && qualityRandom >= 76)
            {
                //Rarity 3
            }
            if (qualityRandom <= 94 && qualityRandom >= 89)
            {
                //Rarity 4
            }
            if (qualityRandom >= 95)
            {
                //Rarity 5
            }
        }
    }
}
