using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPromptSequencer : MonoBehaviour
{
    [SerializeField] List<TextPrompt> promptQueue = new();
    int currentElementIndex = 0;


    public void NextActionPrompt()
    {
        
    }

    public TextPrompt DequeuePrompt()
    {
        if (promptQueue.Count == 0)
            return null;

        TextPrompt prompt = promptQueue[0];
        promptQueue.RemoveAt(0);
        return prompt;
    }

   

}







