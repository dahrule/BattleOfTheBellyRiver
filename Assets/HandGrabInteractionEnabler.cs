//using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands.Samples.VisualizerSample;
using UnityEngine.XR.Interaction.Toolkit;

public class HandGrabInteractionEnabler : MonoBehaviour
{
    [SerializeField] HandVisualizer handVisualizer;
    [SerializeField] XRRayInteractor[] RayInteractors;

    [SerializeField] private bool _handInteractionEnabledAtStart = true;

    void Start()
    {
        EnableHandGrabInteraction(_handInteractionEnabledAtStart);
    }

    public void EnableHandGrabInteraction(bool enabled)
    {
        if(handVisualizer!=null)
            handVisualizer.drawMeshes = enabled;

        foreach (var item in RayInteractors)
        {
            if (item != null)
                item.enabled = enabled;
        }
    }
}
