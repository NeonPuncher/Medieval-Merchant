using System.Collections.Generic;

using FarrokhGames.Inventory;

using UnityEngine;

[CreateAssetMenu(fileName = "HaggleAbility", menuName = "MedievalMerchant/Haggle Ability", order = 1)]
public class Script_HaggleAbility : ScriptableObject
{
    [Header("Card Visuals")]

    [SerializeField] public string cardName;
    [SerializeField] public Sprite cardImage;
    [SerializeField] public int cardType;
    [SerializeField] public float changeValue;
    [SerializeField] public float angerValue;
    [SerializeField] public Color bannerColor;

    [Header("Grid Variables")]
    [SerializeField] public string itemName;
    [SerializeField] public int gridShape;
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
