using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

using static UnityEditor.Progress;

public class Shop_QuestManager : MonoBehaviour
{

    //Variables for populating and creating quest prefabs in info book
    private string itemTask;
    private string nameTask;
    public GameObject prefabTask;
    public Transform infoParent;

    //Lists of all active and completed quests
    public List<Script_Quest> activeQuests = new List<Script_Quest>();
    private List<Script_Quest> completedQuests = new List<Script_Quest>();

    //NPC Spawner
    public Shop_CustomerSpawn spawner;
    public TimeManager time;

    private void Start()
    {
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
            FillTasks(quest.questName, quest.npcName, quest.requestItem, quest.dayCompleted, quest.dayAccepted);
        }
    }

    //Fill a task prefab with the right information
    public void FillTasks(string questName, string name, string item, int completionTime, int acceptedTime)
    {
        GameObject infoTask = Instantiate(prefabTask, infoParent);
        infoTask.name = questName;
        TextMeshProUGUI[] infoText = infoTask.GetComponentsInChildren<TextMeshProUGUI>();
        infoText[0].text = name;
        infoText[1].text = "Craft: " + item;
        infoText[2].text = (acceptedTime + completionTime - time.totalDay).ToString();
    }

    //Create a scriptable object with the right properties
    public void AcceptQuest(int prefab, string questName, string npcName, string requestItem, ScriptRace race, Script_Job job, string[] sentences)
    {
        Script_Quest quest = ScriptableObject.CreateInstance<Script_Quest>();
        quest.npcPrefab = prefab;
        quest.name = questName;
        quest.questName = questName;
        quest.npcName = npcName;
        quest.race = race;
        quest.job = job;
        quest.requestItem = requestItem;
        quest.dayAccepted = time.totalDay;
        quest.dayCompleted = 2;
        quest.sentences = sentences;

        activeQuests.Add(quest);
        Debug.Log("Quest added: " + questName);
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
        foreach(Script_Quest quest in activeQuests)
        {
            if((quest.dayAccepted + quest.dayCompleted - time.totalDay) == 0)
            {
                spawner.SpawnQuestNPC(quest.npcPrefab, quest);
            }
        }
    }
}
