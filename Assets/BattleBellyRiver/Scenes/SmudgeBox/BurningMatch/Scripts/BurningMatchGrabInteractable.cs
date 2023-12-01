using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(BurningMatch))]
public class BurningMatchGrabInteractable : XRGrabInteractable
{
    [Header("BurningMatch Specifics")]
    [Tooltip("The Interaction Layer that changes based on the burning match lit state")]
    [SerializeField] private string dynamicLayer = "Fire";

    private BurningMatch match;

    protected override void Awake()
    {
        base.Awake();
        match = GetComponent<BurningMatch>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(match!=null)
        {
            match.OnIsLitChanged += ChangeInteractionLayerMask;
        }
        
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if(match!=null)
        {
            match.OnIsLitChanged -= ChangeInteractionLayerMask;
        }
    }

    private bool IsLayerInMask(InteractionLayerMask mask, string layerName)
    {
        int layerValue = InteractionLayerMask.NameToLayer(layerName);
        return (mask & (1 << layerValue)) != 0;
    }

    private void RemoveLayerFromMask(InteractionLayerMask mask, string layerName, out InteractionLayerMask result)
    {
        int layerValue = InteractionLayerMask.NameToLayer(layerName);
        int layerMaskToRemove = 1 << layerValue;
        layerMaskToRemove = ~layerMaskToRemove;
        result = mask & layerMaskToRemove;
    }

    private void AddLayerToMask(InteractionLayerMask mask, string layerName, out InteractionLayerMask result)
    {
        int layerValue = InteractionLayerMask.NameToLayer(layerName);
        int layerMaskToAdd = 1 << layerValue;
        result = mask | layerMaskToAdd;
    }

    //OnIsLitChanged event callback
    void ChangeInteractionLayerMask(bool IsLit)
    {
        InteractionLayerMask newLayers;
        if (IsLit)
        {
            AddLayerToMask(this.interactionLayers, dynamicLayer, out newLayers);
        }
        else
        {
            RemoveLayerFromMask(this.interactionLayers, dynamicLayer, out newLayers);
        }

        this.interactionLayers = newLayers;
    }
}
