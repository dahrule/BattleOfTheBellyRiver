using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR.Interaction.Toolkit;

public class SmudgeBox : MonoBehaviour
{
    [SerializeField] XRSocketInteractor [] smudgeItemSockets;
    [SerializeField] GameObject[] smudgeItemGameobjects;
    [SerializeField] VisualEffect smokeVfx;
    [SerializeField] float delaySeconds = 4f;

    private int smudgeItemCount = 0;
    private readonly int smudgeItemsNeeded=3;

    private void Start()
    {
        smokeVfx.gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        for (int i = 0; i < smudgeItemSockets.Length; i++)
        {
            int currentIndex = i;
            smudgeItemSockets[currentIndex].selectEntered.AddListener((SelectEnterEventArgs args) => OnSelectEnteredCallback(args, currentIndex));
            smudgeItemSockets[currentIndex].selectExited.AddListener((SelectExitEventArgs args) => OnSelectExitedCallback(args, currentIndex));
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < smudgeItemSockets.Length; i++)
        {
            int currentIndex = i;
            smudgeItemSockets[currentIndex].selectEntered.RemoveListener((SelectEnterEventArgs args) => OnSelectEnteredCallback(args, currentIndex));
            smudgeItemSockets[currentIndex].selectExited.RemoveListener((SelectExitEventArgs args) => OnSelectExitedCallback(args, currentIndex));
        }
    }

    private void OnSelectEnteredCallback(SelectEnterEventArgs args, int currentIndex)
    {
        AddSmudgeItem();
        HideGhostItem(smudgeItemGameobjects[currentIndex]);
    }

    private void OnSelectExitedCallback(SelectExitEventArgs args, int currentIndex)
    {
        RemoveSmudgeItem();
        ShowGhostItem(smudgeItemGameobjects[currentIndex]);
    }

    private void AddSmudgeItem()
    {
        smudgeItemCount += 1;

        bool isMixComplete = smudgeItemCount == smudgeItemsNeeded;
        if (isMixComplete)
            Invoke(nameof(StartSmudgeFire), delaySeconds);
    }

    private void RemoveSmudgeItem()
    {
        smudgeItemCount -= 1;
    }

    private void HideGhostItem(GameObject ghostItem)
    {
        ghostItem.SetActive(false);
    }

    private void ShowGhostItem(GameObject ghostItem)
    {
        ghostItem.SetActive(true);
    }

    private void StartSmudgeFire()
    {
        Debug.Log("Starting smudge fire");
        smokeVfx.gameObject.SetActive(true);
        //play a success sound
        this.enabled = false;
    }

}






