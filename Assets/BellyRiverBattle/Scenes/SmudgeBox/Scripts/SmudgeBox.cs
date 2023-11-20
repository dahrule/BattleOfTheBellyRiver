using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SmudgeBox : MonoBehaviour
{
    [SerializeField] XRSocketInteractor [] SmudgeItems;
    [SerializeField] ParticleSystem smokeVfx;
    [SerializeField] ParticleSystem fireVfx;

    private int smudgeItemCount = 0;
    private readonly int smudgeItemsNeeded=3;

    private void OnEnable()
    {
        foreach(var item in SmudgeItems)
        {
            item.selectEntered.AddListener(AddSmudgeItem);
            item.selectExited.AddListener(RemoveSmudgeItem);
        }
    }

    private void OnDisable()
    {
        foreach (XRSocketInteractor socket in SmudgeItems)
        {
            socket.selectEntered.RemoveListener(AddSmudgeItem);
            socket.selectExited.RemoveListener(RemoveSmudgeItem);
        }
    }

    private void AddSmudgeItem(SelectEnterEventArgs arg0)
    {
        smudgeItemCount += 1;

        bool isMixComplete = smudgeItemCount == smudgeItemsNeeded;
        if (isMixComplete)
            StartSmudgeFire();
    }

    private void RemoveSmudgeItem(SelectExitEventArgs arg0)
    {
        smudgeItemCount -= 1;
    }

    private void StartSmudgeFire()
    {
        Debug.Log("Starting smudge fire");
        //Trigger smoke 
        //play a success sound
        //add a mechanismo to avoid retriggering the event twice starign fire
    }

}
