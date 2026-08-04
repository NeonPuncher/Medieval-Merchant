using TMPro;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Haggle_Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image cardSprite;
    public Image bannerSprite;
    public TextMeshProUGUI cardName;
    public int cardType;
    private Haggle_ItemManager itemManager;
    public Animator cardAnimation;
    public Script_HaggleCard cardObj;

    private void Start()
    {
        itemManager = FindFirstObjectByType<Haggle_ItemManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardAnimation.SetBool("CardHover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardAnimation.SetBool("CardHover", false);
    }

    public void OnCardClick()
    {
        switch (cardType)
        {
            case 0:
                {
                    itemManager.ChangeAnger(cardObj);
                    break;
                }
            case 1:
                {
                    itemManager.IncreasePrice(.2f);
                    break;
                }
            case 2:
                {
                    itemManager.LowerPrice(.2f);
                    break;
                }
            case 3:
                {
                    itemManager.IncreasePrice(.1f);
                    itemManager.ChangeAnger(cardObj);
                    break;
                }
        }

        Destroy(this.gameObject);
    }
}
