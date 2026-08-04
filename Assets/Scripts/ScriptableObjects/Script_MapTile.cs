using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "MedievalMerchant/MapTile", order = 1)]
public class Script_MapTile : ScriptableObject
{
    public string mapName;
    public string ID;
    public List<Script_InvItem> rewards;
    public float workTime;
    public int cost;
    public List<int> workReq;
    public float workDiff;
    public Sprite mapImage;
}
