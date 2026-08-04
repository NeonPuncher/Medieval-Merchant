using NUnit.Framework;

using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MedievalMerchant/Race", order = 1)]
public class ScriptRace : ScriptableObject
{
    public string raceName;
    public string[] raceNames;
    public float hagDif;
    public string[] rewards;

    public Script_HaggleCard[] compliments; 

    [TextArea(3, 10)]
    public string[] dialogueIntro;

    [TextArea(3, 10)]
    public string[] dialogueAccept;

    [TextArea(3, 10)]
    public string[] dialogueRefuse;

    [TextArea(3, 10)]
    public string[] dialogueReturn;

    [TextArea(3, 10)]
    public string[] dialogueComplete;
}
