using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ArtefactGrabInteractable : XRGrabInteractable
{
    [Header("Scriptable Object")]
    [SerializeField] Artefact artefact;

    public static event UnityAction<Artefact> OnObjectSelected;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Raise the OnObjectSelected event
        OnObjectSelected?.Invoke(this.artefact);
    }
}

