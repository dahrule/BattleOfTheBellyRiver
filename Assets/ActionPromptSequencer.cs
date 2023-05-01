using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPromptSequencer : MonoBehaviour
{
    [SerializeField] List<ActionPrompt> promptQueue = new();
    int currentElementIndex = 0;


    public void NextActionPrompt()
    {
        
    }

    public ActionPrompt DequeuePrompt()
    {
        if (promptQueue.Count == 0)
            return null;

        ActionPrompt prompt = promptQueue[0];
        promptQueue.RemoveAt(0);
        return prompt;
    }

   

}







