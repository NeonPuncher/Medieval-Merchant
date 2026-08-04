using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MedievalMerchant/Job", order = 1)]
public class Script_Job : ScriptableObject
{
    public string jobName;
    public string[] items;
    public string[] rewards;
}
