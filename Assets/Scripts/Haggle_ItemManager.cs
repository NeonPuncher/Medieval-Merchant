using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Haggle_ItemManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI offerText;
    [SerializeField] private Button raiseButton;
    [SerializeField] private Image haggleFill;

    [SerializeField] private List<Script_HaggleCard> haggleCards;
    public GameObject haggleCardPrefab;
    public Transform haggleCardParent;

    [SerializeField] private Animator handAnim;

    public Script_Quest questNPC;

    public string itemName;
    public int itemValue;
    public int totalitems;

    //Variables to run code when the total value changes
    public float totalvalue;
    public float totalValue
    {
        get
        {
            return totalvalue;
        }
        set
        {
            totalvalue = value;
            UpdateOffer();
        }
    }
    public float offerValue;
    public float totalQual;
    public float anger;
    private float raiseChance;

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("HaggleScene"));
        ////questNPC = FindAnyObjectByType<InventoryTransfer>().npcQuest;
        //itemName = questNPC.requestItem;

        changeHaggleHealth(3);
        offerText.text = offerValue.ToString();

        for (int i = 0; i < 5; i++)
        {
            Script_HaggleCard randomCard = haggleCards[Random.Range(0, haggleCards.Count)];
            GameObject HaggleCard = Instantiate(haggleCardPrefab, haggleCardParent);

            HaggleCard.GetComponent<Haggle_Card>().cardName.text = randomCard.cardName;
            HaggleCard.GetComponent<Haggle_Card>().cardSprite.sprite = randomCard.cardImage;
            HaggleCard.GetComponent<Haggle_Card>().cardType = randomCard.cardType;
            HaggleCard.GetComponent<Haggle_Card>().cardObj = randomCard;
            HaggleCard.GetComponent<Haggle_Card>().bannerSprite.color = randomCard.bannerColor;
        }
    }
    
    //Update the offer if the total value changes
    public void UpdateOffer()
    {
        //if(itemName == questNPC.requestItem)
        //{
        //    //Item is the specific requested item
        //    offerValue = offerValue + Random.Range(itemValue * .8f, itemValue * 1.2f); ;
        //}
        //else
        //{
        //    //Item is any random Item
        //    offerValue = offerValue + Random.Range(itemValue * .5f, itemValue + 1);
        //}

        handAnim.SetTrigger("HandMove");

        //Round offer value to whole
        offerValue = Mathf.Round(offerValue);
        offerText.text = offerValue.ToString();
    }

    //Raise offer based on total value, item qual, offer value, and emotion of npc
    //If failed too many times button will deactivate 
    public void RaiseOffer()
    {
        //Lmao void
    }

    //Raises offer
    public void IncreasePrice(float increaseAmount)
    {

        float chance = Random.Range(0, 1);
        if(chance <= ((anger * .2f) -.1f))
        {
            offerValue = offerValue + (Mathf.Round(offerValue * increaseAmount));
            offerText.text = offerValue.ToString();
        }
        else
        {
            LowerPrice(.2f);
        }
    }

    public void LowerPrice(float lowerAmount)
    {
        offerValue = offerValue - (Mathf.Round(offerValue * lowerAmount));
        changeHaggleHealth(5);
        offerText.text = offerValue.ToString();
    }

    public void ChangeAnger(Script_HaggleCard card)
    {
        if(questNPC.race.compliments.Contains(card))
        {
            //Right Compliment
            Debug.Log("Compliment given");
            changeHaggleHealth(anger + 1);
        }
        else
        {
            //Wrong compliment
            Debug.Log("Your mom");
            changeHaggleHealth(anger - 1);
        }
    }

    void changeHaggleHealth(float angerAmount)
    {
        anger = angerAmount;
        haggleFill.fillAmount = angerAmount * .2f;

        if(anger == 0)
        {
            Debug.Log("You're dead lmao");
            ChangeScene();
        }
    }

    // Update is called once per frame
    public void ChangeScene()
    {
        
        PlayerPrefs.SetFloat("money", (PlayerPrefs.GetFloat("money") + offerValue));

        InventoryTransfer transfer = FindAnyObjectByType<InventoryTransfer>();
        transfer.ReloadMainShop();
        SceneManager.UnloadSceneAsync("HaggleScene");
    }
}
