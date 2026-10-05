using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "MedievalMerchant/Job", order = 1)]
public class Script_Job : ScriptableObject
{
    public string jobName;
    public string[] items;
    public string[] rewards;
    public List<Sprite> dialogueSprite;

    [SerializeField] public TextAsset inkJson;
}
