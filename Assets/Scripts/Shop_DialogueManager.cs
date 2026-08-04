using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

public class Shop_DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    public TextMeshProUGUI dialogueText;
    public GameObject completeButton;
    public GameObject inventoryCustomer;
    public Animator cameraAnim;

    public Animator animator;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Shop_Dialogue dialogue, int DialogueCase)
    {
        cameraAnim.SetBool("ZoomIn", true);
        animator.SetBool("EndDialogue", false);
        animator.SetBool("IsOpen", true);
        sentences.Clear();

        switch (DialogueCase)
        {
            case 0:
                //Normal Start
                sentences.Clear();
                sentences.Enqueue(dialogue.sentences[DialogueCase]);
                break;
            
            case 1:
                //AcceptDialogue
                animator.SetBool("IsChoice", true);
                sentences.Clear();
                sentences.Enqueue(dialogue.sentences[DialogueCase]);
                DisplayNextSentence();
                break;

            case 2:
                //Refuse Dialogue
                animator.SetBool("IsChoice", true);
                sentences.Clear();
                sentences.Enqueue(dialogue.sentences[DialogueCase]);
                DisplayNextSentence();
                break;

            case 3:
                //Reoccuring NPC
                animator.SetBool("IsComplete", true);
                sentences.Clear();
                sentences.Enqueue(dialogue.sentences[DialogueCase]);
                completeButton.SetActive(true);
                break;

            case 4:
                //Complete Dialogue
                animator.SetBool("IsComplete", false);
                animator.SetBool("IsChoice", true);
                sentences.Clear();
                sentences.Enqueue(dialogue.sentences[DialogueCase]);
                DisplayNextSentence();
                break;
        }

        DisplayNextSentence();
    }

    //Display next sentence
    public void DisplayNextSentence()
    {
        Debug.Log(sentences.Count);
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    //Typing the sentence letter by letter
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.01f);
        }
    }

    //Player accepts quest, pleasing the customer, or denies and pisses off customer
    public void AcceptDialogue(Shop_Dialogue dialogue, int dialogueLine)
    {
        animator.SetBool("IsChoice", true);
        sentences.Clear();
        sentences.Enqueue(dialogue.sentences[dialogueLine]);
        DisplayNextSentence();
    }

    //Player completes the quest
    public void CompleteDialogue(Shop_Dialogue dialogue, int dialogueLine)
    {
        sentences.Clear();
        animator.SetBool("IsComplete", false);
        animator.SetBool("IsChoice", true);
        sentences.Enqueue(dialogue.sentences[dialogueLine]);
        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        animator.SetBool("IsComplete", false);
        animator.SetBool("IsChoice", false);
        animator.SetBool("IsOpen", false);
        animator.SetBool("EndDialogue", true);
        cameraAnim.SetBool("ZoomIn", false);
    }

}
