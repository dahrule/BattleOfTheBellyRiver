using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ArtefactGrabInteractable : XRGrabInteractable
{
    [Header("Artefact Data")]
    [SerializeField] Artefact artefact;

    public static event UnityAction<Artefact> OnObjectSelected;
    public static event UnityAction<Artefact> OnObjectReleased;

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

    /*protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        OnObjectSelected?.Invoke(this.artefact);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        OnObjectReleased?.Invoke(this.artefact);
    }*/
}

