using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using FarrokhGames.Inventory;

[CreateAssetMenu(fileName = "Data", menuName = "MedievalMerchant/Worker", order = 1)]

public class Script_Worker : ScriptableObject
{
    //Worker info variables
    [Header("Worker Variables")]

    [SerializeField] public Sprite workerSprite = null;
    [SerializeField] public int workerID;
    [SerializeField] public string workerName;
    [SerializeField] public int workerCost;
    [SerializeField] public int workerSkillScav;
    [SerializeField] public int workerSkillSurv;
    [SerializeField] public int workerSkillFight;
    [SerializeField] public float workerSkillSpeed;
}
