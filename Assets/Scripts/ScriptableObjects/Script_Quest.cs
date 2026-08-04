using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "MedievalMerchant/Quest", order = 1)]
public class Script_Quest : ScriptableObject
{
    public int npcPrefab;
    public string questName;
    public ScriptRace race;
    public Script_Job job;
    public string npcName;
    public string requestItem;
    public int dayAccepted;
    public int dayCompleted;

    public string[] sentences;
}
