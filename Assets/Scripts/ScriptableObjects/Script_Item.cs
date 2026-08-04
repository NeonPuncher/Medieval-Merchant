using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "MedievalMerchant/Item", order = 1)]
public class Script_Item : ScriptableObject
{
    public string itemName;
    public string itemType;
    public int attachAmount;
    public float itemCost;
    public float itemQual;
    public Sprite itemImage; 
}