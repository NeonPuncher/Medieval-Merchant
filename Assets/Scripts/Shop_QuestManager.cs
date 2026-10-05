using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

using static UnityEditor.Progress;

public class Shop_QuestManager : MonoBehaviour
{

    //Variables for populating and creating quest prefabs in info book
    private string nameTask;
    public GameObject prefabTask;
    public Transform infoParent;

    //Lists of all active and completed quests
    public List<Script_Quest> activeQuests = new List<Script_Quest>();
    private List<Script_Quest> completedQuests = new List<Script_Quest>();

    //NPC reward variables
    public Inventory_Size NPCInventory;
    public GameObject NPCInventory_UI;

    //NPC Spawner
    public Shop_CustomerSpawn spawner;
    public TimeManager time;
    private int TotalTasks;

    private void Start()
    {
        TotalTasks = 0;
        time = FindAnyObjectByType<TimeManager>();
        spawner = FindAnyObjectByType<Shop_CustomerSpawn>();    
    }

    public void FillQuestLog()
    {
        foreach (Transform child in infoParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Script_Quest quest in activeQuests)
        {
            //MAKE QUEST ICON IN LOGBOOK
            FillTasks(quest.npcName, quest.dayCompleted, quest.dayAccepted);
        }
    }

    //Fill a task prefab with the right information
    public void FillTasks(string name, int completionTime, int acceptedTime)
    {
        GameObject infoTask = Instantiate(prefabTask, infoParent);
        infoTask.name = name;
        TextMeshProUGUI[] infoText = infoTask.GetComponentsInChildren<TextMeshProUGUI>();
        infoText[0].text = name;
        infoText[2].text = (acceptedTime + completionTime - time.totalDay).ToString();
    }

    //Create a scriptable object with the right properties
    public void CreateQuest(string questName, int prefab, string npcName, ScriptRace race, Script_Job job, int questState)
    {
        Script_Quest quest = ScriptableObject.CreateInstance<Script_Quest>();
        quest.name = questName;
        quest.npcPrefab = prefab;
        quest.npcName = npcName;
        quest.race = race;
        quest.job = job;
        quest.dayAccepted = time.totalDay;
        quest.dayCompleted = 1;
        quest.questState = questState;

        activeQuests.Add(quest);
        TotalTasks++;
        Debug.Log(TotalTasks);
    }

    //Remove quest from active quest list
    public void CompleteQuest(Script_Quest questName)
    {
        if(activeQuests.Contains(questName))
        {
            activeQuests.Remove(questName);
            completedQuests.Add(questName);
            Debug.Log("Quest completed: " + questName);

        }
    }

    //Spawn quest NPC at right day
    public void SpawnQuestNPC()
    {
        Debug.Log("Initialize Quest NPCs");
        foreach(Script_Quest quest in activeQuests)
        {
            if((quest.dayAccepted + quest.dayCompleted - time.totalDay) == 0)
            {
                spawner.SpawnQuestNPC(quest.npcPrefab, quest);
            }
        }
    }

    public void GiveRewardItems(Script_Quest quest)
    {
        if(quest.hasReward == true)
        {
            //If money reward put here 
            //ADD quest.rewardGold to global gold variable
            if(quest.rewardItems != null)
            {
                NPCInventory_UI.SetActive(true);
                NPCInventory.AddSpecificItem(quest.rewardItems);
            }
        }
    }
}
