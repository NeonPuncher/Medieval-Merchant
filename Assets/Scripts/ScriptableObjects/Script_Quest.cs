using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MedievalMerchant/Quest", order = 1)]
public class Script_Quest : ScriptableObject
{
    public int npcPrefab;
    public ScriptRace race;
    public Script_Job job;
    public string npcName;
    public int dayAccepted;
    public int dayCompleted;
    public int questState;

    //Is there a reward
    public bool hasReward;
    public float rewardGold;
    public List<Script_InvItem> rewardItems;

    //Variables for Quest Quality Item and whether it was succesfull 
    public float questQuality;
    public bool qualityMet;
}
