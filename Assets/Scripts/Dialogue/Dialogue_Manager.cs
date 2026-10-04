using UnityEngine;
using Ink.Runtime;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class Dialogue_Manager : MonoBehaviour
{
    private bool dialoguePlaying = false;
    public Dialogue_Events dialogue_Events;
    public Dialogue_UI dialogueUI;

    [Header("Ink Story")]
    [SerializeField] public TextAsset inkJson;
    public string lastKnotName;
    private Story story;
    private int currentChoiceIndex = -1;
    private const string PortraitTag = "portrait";

    private void Awake()
    {
        dialogue_Events = new Dialogue_Events();
        dialogueUI = FindAnyObjectByType<Dialogue_UI>();
        story = new Story(inkJson.text);
    }

    public void DialogueStart(string knotName)
    {
        if (!knotName.Equals(""))
        {
            if (dialoguePlaying == true)
            {
                dialogue_Events.PressedSubmit();
            }
            dialogue_Events.EnterDialogue(knotName);
        }
    }

    private void OnEnable()
    {
        dialogue_Events.onEnterDialogue += EnterDialogue;
        dialogue_Events.onPressedSubmit += SubmitPressed;
        dialogue_Events.onUpdateChoiceIndex += UpdateChoiceIndex;
    }

    private void OnDisable()
    {
        dialogue_Events.onEnterDialogue -= EnterDialogue;
        dialogue_Events.onPressedSubmit -= SubmitPressed;
        dialogue_Events.onUpdateChoiceIndex -= UpdateChoiceIndex;
    }

    private void UpdateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }

    public void SubmitPressed()
    {
        if(!dialoguePlaying)
        {
            return;
        }

        ContinueOrExitStory();
    }

    private void EnterDialogue(string knotName)
    {
        if (dialoguePlaying)
        {
            return;
        }
        dialoguePlaying = true;

        dialogue_Events.DialogueStarted();

        if(!knotName.Equals(""))
        {
            story.ChoosePathString(knotName); 
        }
        else
        {
            Debug.LogWarning("Knot name was the empty string when entering");
        }

        ContinueOrExitStory();
    }

    private void ContinueOrExitStory()
    {
        if(story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);
            currentChoiceIndex = -1;
        }

        if (story.canContinue)
        {
            string dialogueLine = story.Continue();
            dialogue_Events.DisplayDialogue(dialogueLine, story.currentChoices);
            HandleTags(story.currentTags);
        }
        else if (story.currentChoices.Count == 0) 
        {
            StartCoroutine(ExitDialogue());
        }
    }


    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            //parse tag
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be parsed" + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            //handle tag
            switch (tagKey)
            {
                case PortraitTag:
                    dialogueUI.ChangePortrait(int.Parse(tagValue));
                    break;
                default:
                    Debug.LogWarning("Tag cannot be handled" + tag);
                    break;
            }
        }
    }

    private IEnumerator ExitDialogue()
    {
        yield return null;

        dialoguePlaying = false;
        dialogue_Events.DialogueFinished();
        story.ResetState();
    }
}
