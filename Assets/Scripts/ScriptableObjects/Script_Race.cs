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
}
