using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Shop_NPC : MonoBehaviour
{
    [Header("Editor Variables")]
    [SerializeField] private ScriptRace scriptRace;
    [SerializeField] private string knotName;
    [SerializeField] private Script_Job job;
    [SerializeField] private GameObject questMark;

    private Shop_QuestManager questManager;
    private Dialogue_Manager dialogueManager;
    private Shop_CustomerMove customerMove;
    private Dialogue_UI dialogueUI;
    public Script_Quest quest;
    private bool hasQuest = false;
    private bool isAccepted;

    private Button tradeButton;
    private Button delayButton;
    private Button refuseButton;

    [Header("NPC Variables")]
    [SerializeField] public string NPCName;
    [SerializeField] public int prefabNum;
    [SerializeField] public bool isReoccuring;
    [SerializeField] public float goldAmount;
    [SerializeField] public float questQuality;
    [SerializeField] public float haggleDifficult;

    [SerializeField] private int questState;

    private void Start()
    {
        customerMove = this.GetComponent<Shop_CustomerMove>();
        questManager = FindAnyObjectByType<Shop_QuestManager>();
        dialogueManager = FindAnyObjectByType<Dialogue_Manager>();
        dialogueUI = FindAnyObjectByType<Dialogue_UI>();

        //Set entrypoint of dialogue

        if(hasQuest == false)
        {
            StartCoroutine(PauseForQuest());
        }

        //Initialize NPC Variables if they are from quest object
        if (quest != null)
        {
            NPCName = quest.npcName;
            questState = quest.questState;
            knotName = quest.npcName + questState;
        }

        //Assign Buttons
        tradeButton = dialogueUI.dialogueButtons[0];
        tradeButton.onClick.AddListener(TradeTask);

        delayButton = dialogueUI.dialogueButtons[1];
        delayButton.onClick.AddListener(DelayTask);

        refuseButton = dialogueUI.dialogueButtons[2];
        refuseButton.onClick.AddListener(RefuseTask);

        //When coming back from haggle scene, open dialogue
    }

    private void OnMouseDown()
    {
        if (hasQuest)
        {
            if(dialogueManager.lastKnotName != knotName && quest.job.dialogueSprite != null)
            {
                dialogueUI.portraitSprites = quest.job.dialogueSprite;
            }
            dialogueManager.DialogueStart(knotName);
        }
    }

    //Wait until quest will appear
    IEnumerator PauseForQuest()
    {
        yield return new WaitForSeconds(Random.Range(3, 20));
        hasQuest = true;
        questMark.SetActive(true);
    }

    //Comes after haggling
    public void CompleteTask()
    {
        if(isReoccuring)
        {
            //Make quest logger and quest tracker
            //Check whether quest quality is met, if met have him return happy state, if not have him return angry state
        }
    }

    public void TradeTask()
    {
        questMark.SetActive(false);
        questManager.GiveRewardItems(quest);
        //Open Haggle Scene
        customerMove.isLeaving = true;
    }
    public void DelayTask()
    {
        questMark.SetActive(false);
        questState = 1;
        if(quest == null)
        {
            //Create new quest if NPC has none
            questManager.CreateQuest(quest.name, prefabNum, NPCName, scriptRace, job, questState);
        }
        else
        {
            //Re-add current quest with questState changed
            questManager.activeQuests.Remove(quest);
            questManager.CreateQuest(quest.name, prefabNum, NPCName, scriptRace, job, questState);
        }
        //MAKE THEM LEAVE
        customerMove.isLeaving = true;
    }

    public void RefuseTask()
    {
        questMark.SetActive(false);
        questManager.activeQuests.Remove(quest);
        customerMove.isLeaving = true;
    }
}
