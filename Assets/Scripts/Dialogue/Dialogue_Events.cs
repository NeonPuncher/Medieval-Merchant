using UnityEngine;
using System;
using Ink.Runtime;
using System.Collections.Generic;
public class Dialogue_Events
{
    public event Action<string> onEnterDialogue;

    public void EnterDialogue(string knotName)
    {
        //Check if onenterdialogue != null
        onEnterDialogue?.Invoke(knotName);
    }

    public event Action onPressedSubmit;
    public void PressedSubmit()
    {
        onPressedSubmit?.Invoke();
    }

    public event Action onDialogueStarted;
    public void DialogueStarted()
    {
        if (onDialogueStarted != null)
        {
            onDialogueStarted();
        }
    }

    public event Action onDialogueFinished;
    public void DialogueFinished()
    {
        if (onDialogueFinished != null)
        {
            {
                onDialogueFinished();
            }
        }
    }

    public event Action<string, List<Choice>> onDisplayDialogue;
    public void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices)
    {
        if(onDisplayDialogue != null)
        {
            onDisplayDialogue(dialogueLine, dialogueChoices);
        }
    }

    //Choice index
    public event Action<int> onUpdateChoiceIndex;
    public void UpdateChoiceIndex(int choiceIndex)
    {
        if (onUpdateChoiceIndex != null)
        {
            onUpdateChoiceIndex(choiceIndex);
        }
    }
}
