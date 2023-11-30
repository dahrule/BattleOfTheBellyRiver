using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ArtefactGrabInteractable : XRGrabInteractable
{
    [Header("Artefact Data")]
    [SerializeField] Artefact artefact;
    [Tooltip("Who can interact with this object")]
    [SerializeField] string interactorTag;

    public static event UnityAction<Artefact> OnObjectSelected;
    public static event UnityAction<Artefact> OnObjectReleased;

    private Vector3 positionAtStart;
    private Quaternion rotationAtStart;

    private void Start()
    {
        positionAtStart = this.transform.position;
        rotationAtStart = this.transform.rotation;
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        //Trigger event when the interactor is the player.
        GameObject player = args.interactorObject.transform.gameObject;
        if (player.CompareTag(interactorTag))
        { 
            OnObjectSelected?.Invoke(this.artefact); 
        } 
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        //Trigger event when the interactor is the player.
        GameObject player = args.interactorObject.transform.gameObject;
        if (player.CompareTag(interactorTag))
        {
            OnObjectReleased?.Invoke(this.artefact);
        }
    }

    public void ResetTransform()
    {
        this.transform.SetPositionAndRotation(positionAtStart, rotationAtStart);
    }
}

