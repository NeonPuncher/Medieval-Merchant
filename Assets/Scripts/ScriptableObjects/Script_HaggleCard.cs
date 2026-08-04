using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MedievalMerchant/HaggleCards", order = 1)]

public class Script_HaggleCard : ScriptableObject
{
    [Header("Card Visuals")]

    [SerializeField] public string cardName;
    [SerializeField] public Sprite cardImage;
    [SerializeField] public int cardType;
    [SerializeField] public float changeValue;
    [SerializeField] public float angerValue;
    [SerializeField] public Color bannerColor;
}
