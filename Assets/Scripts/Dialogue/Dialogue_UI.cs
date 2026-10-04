using System.Collections;
using System.Collections.Generic;

using Ink.Runtime;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class Dialogue_UI : MonoBehaviour
{
    public Dialogue_Events dialogue_Events;

    [Header("Components")]
    [SerializeField] private float typingSpeed = 0.04f;
    [SerializeField] private GameObject dialogueParent;
    [SerializeField] public GameObject buttonParent;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image portrait;
    [SerializeField] public List<Sprite> portraitSprites;
    [SerializeField] private Animator UIanimation;
    [SerializeField] public List<Button> dialogueButtons;

    private Coroutine displayLineCoroutine;

    private void Awake()
    {
        dialogueParent.SetActive(false);
        buttonParent.SetActive(false);
        ResetPanel();
    }

    private void Start()
    {
        dialogue_Events = FindAnyObjectByType<Dialogue_Manager>().dialogue_Events;
        dialogue_Events.onDialogueStarted += DialogueStarted;
        dialogue_Events.onDialogueFinished += DialogueFinished;
        dialogue_Events.onDisplayDialogue += DisplayDialogue;
    }

    private void OnDisable()
    {
        dialogue_Events.onDialogueStarted -= DialogueStarted;
        dialogue_Events.onDialogueFinished -= DialogueFinished;
        dialogue_Events.onDisplayDialogue -= DisplayDialogue;
    }

    private void OnMouseDown()
    {
        //On UI Click go to next dialogue
    }

    private void DialogueStarted()
    {
        dialogueParent.SetActive(true);
        dialogueParent.GetComponent<Button>().onClick.AddListener(FindAnyObjectByType<Dialogue_Manager>().SubmitPressed);
        UIanimation.SetTrigger("Open");
    }

    private void DialogueFinished()
    {
        UIanimation.SetTrigger("Close");
        dialogueParent.GetComponent<Button>().onClick.RemoveListener(FindAnyObjectByType<Dialogue_Manager>().SubmitPressed);
        dialogueParent.SetActive(false);
        ResetPanel();
    }

    private void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices)
    {
        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
        }

        if(dialogueChoices.Count > 0)
        {
            buttonParent.SetActive(true);
            UIanimation.SetBool("Choices", true);
        }
        else
        {
            UIanimation.SetBool("Choices", false);
        }

        displayLineCoroutine = StartCoroutine(DisplayLine(dialogueLine));
    }

    private IEnumerator DisplayLine(string line)
    {
        //empty the dialogue
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void ResetPanel()
    {
        dialogueText.text = "";
    }

    public void DialogueChoiceSelect(int ButtonNum)
    {
        dialogue_Events.UpdateChoiceIndex(ButtonNum);
    }

    public void ChangePortrait(int portraitEmote)
    {
        portrait.sprite = portraitSprites[portraitEmote];
    }
}
