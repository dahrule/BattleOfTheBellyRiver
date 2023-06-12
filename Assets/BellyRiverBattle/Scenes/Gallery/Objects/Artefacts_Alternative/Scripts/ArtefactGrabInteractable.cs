using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ArtefactGrabInteractable : XRGrabInteractable
{
    [Header("Artefact Data")]
    [SerializeField] Artefact artefact;

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
        OnObjectSelected?.Invoke(this.artefact);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        OnObjectReleased?.Invoke(this.artefact);
    }

    public void ResetTransform()
    {
        this.transform.SetPositionAndRotation(positionAtStart, rotationAtStart);
    }
}

