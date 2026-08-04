using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Customer : MonoBehaviour
{
    public ScriptRace scriptRace;
    public List<Script_Job> scriptJob;
    public UI_Management uimanager;
    public Shop_QuestManager questManager;
    public Shop_Dialogue dialogue;

    public Script_Job job;
    public string item;
    public int taskNum;
    public int prefabNum;
    public Button acceptButton;
    public Button refuseButton;
    public Button completeButton;

    public Script_Quest quest;
    private bool hasQuest;
    public bool isAccepted;
    public GameObject questMark;

    // Start is called before the first frame update
    void Start()
    {
        uimanager = FindAnyObjectByType<UI_Management>();
        questManager = FindAnyObjectByType<Shop_QuestManager>();

        //FindButtons
        acceptButton = GameObject.FindWithTag("acceptButton").GetComponent<Button>();
        refuseButton = GameObject.FindWithTag("refuseButton").GetComponent<Button>();
        completeButton = GameObject.FindWithTag("completeButton").GetComponent<Button>();

        //SET VARIABLES IF NOT REOCCURING NPC
        if (!isAccepted)
        {
            dialogue.name = scriptRace.raceNames[Random.Range(0, scriptRace.raceNames.Length)];
            job = scriptJob[Random.Range(0, scriptJob.Count)];
            item = job.items[Random.Range(0, item.Length)];
            acceptButton.interactable = true;
            refuseButton.interactable = true;
            completeButton.interactable = true;
        }
        else
        {
            dialogue.name = quest.npcName;
            job = quest.job;
            item = quest.requestItem;
            acceptButton.interactable = false;
            refuseButton.interactable = false;
        }

        dialogue.sentences[0] = scriptRace.dialogueIntro[Random.Range(0, scriptRace.dialogueIntro.Length)];
        dialogue.sentences[0] = ReplaceVariables(dialogue.sentences[0]);

        dialogue.sentences[1] = scriptRace.dialogueAccept[Random.Range(0, scriptRace.dialogueAccept.Length)];
        dialogue.sentences[1] = ReplaceVariables(dialogue.sentences[1]);

        dialogue.sentences[2] = scriptRace.dialogueRefuse[Random.Range(0, scriptRace.dialogueRefuse.Length)];
        dialogue.sentences[2] = ReplaceVariables(dialogue.sentences[2]);

        dialogue.sentences[3] = scriptRace.dialogueReturn[Random.Range(0, scriptRace.dialogueReturn.Length)];
        dialogue.sentences[3] = ReplaceVariables(dialogue.sentences[3]);

        dialogue.sentences[4] = scriptRace.dialogueComplete[Random.Range(0, scriptRace.dialogueComplete.Length)];
        dialogue.sentences[4] = ReplaceVariables(dialogue.sentences[4]);

        StartCoroutine(PauseForQuest());
    }


    //What happens when I click on NPC????
    private void OnMouseDown()
    {
        if (hasQuest)
        {
            if (isAccepted == true)
            {
                FindFirstObjectByType<Shop_DialogueManager>().StartDialogue(dialogue, 3);
                completeButton.onClick.AddListener(delegate { completeTask(); });
                FindAnyObjectByType<InventoryTransfer>().npcQuest = quest;
            }
            else
            {
                FindFirstObjectByType<Shop_DialogueManager>().StartDialogue(dialogue, 0);
                acceptButton.onClick.AddListener(delegate { AcceptTask(); });
                refuseButton.onClick.AddListener(delegate { RefuseTask(); });
            }
        }
    }

    //Wait until quest will appear
    IEnumerator PauseForQuest()
    {
        yield return new WaitForSeconds(Random.Range(3, 20));
        hasQuest = true;
        questMark.SetActive(true);
    }

    //Replace Variables in the dialogue with the right string from the Scriptable object
    private string ReplaceVariables(string text)
    {
        return text
            .Replace("1", dialogue.name)
            .Replace("2", job.jobName)
            .Replace("3", item);
    }

    //If you accept the quest
    public void AcceptTask()
    {
        taskNum++;
        questManager.AcceptQuest(prefabNum, taskNum.ToString() + dialogue.name, dialogue.name, item, scriptRace, job, dialogue.sentences);
        FindFirstObjectByType<Shop_DialogueManager>().AcceptDialogue(dialogue, 1);
        questMark.SetActive(false);
        acceptButton.interactable = false;
        refuseButton.interactable = false;

        //MAKE THEM LEAVE
        Destroy(this.gameObject, 1f);
    }

    //If you refuse the quest
    public void RefuseTask()
    {
        acceptButton.interactable = false;
        refuseButton.interactable = false;
        FindFirstObjectByType<Shop_DialogueManager>().AcceptDialogue(dialogue, 2);

        //MAKE THEM LEAVE
        Destroy(this.gameObject, 1f);
    }

    //If you complete the quest
    public void completeTask()
    {
        completeButton.interactable = false;
        FindFirstObjectByType<Shop_DialogueManager>().CompleteDialogue(dialogue, 4);
        uimanager.ChangeHaggleScene();

        // ADD BARTER MECHANIC
        // MAKE THEM LEAVE
    }
}
