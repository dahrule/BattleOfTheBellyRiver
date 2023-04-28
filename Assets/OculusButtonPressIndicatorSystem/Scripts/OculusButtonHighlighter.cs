using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OculusButtonHighlighter : MonoBehaviour
{
    [SerializeField] OculusControllerType controllerType;
    [SerializeField] XRBaseController controller;
    [SerializeField] XRBaseController hand;
    [Tooltip("Object highlighting the button to be pressed")]
    [SerializeField] GameObject buttonMarkerPrefab;

    [Header("Button-Button Object Mapping")]
    [SerializeField] OculusButton[] keys;
    [Tooltip("Objects representing each button (keys) on the controller.")]
    [SerializeField] Transform[] values;
 
    [Header("Haptic feedback")]
    [Tooltip("Duration of haptic feedback in seconds.")]
    [SerializeField] float duration = 0.6f;
    [Tooltip("How intense the controller vibrates.")]
    [SerializeField] float amplitude = 1.5f;

    readonly Dictionary<OculusButton, Transform> buttonsDict = new();

    private void Awake()
    {
        //Populate the dictionary
        for (int i = 0; i < keys.Length; i++)
        {
            buttonsDict.Add(keys[i], values[i]);
        }
    }
    void Start()
    {
        //ShowController(true);
    }

    void ShowController2(bool show)
    {
        //swap between hand and game controller
        controller.gameObject.SetActive(show);
        hand.gameObject.SetActive(!show);
    }

    [ContextMenu("ShowController")]
    public void ShowController()
    {
        //swap between hand and oculus XRcontroller game objects
        controller.gameObject.SetActive(true);
        hand.gameObject.SetActive(false);
    }

    [ContextMenu("HideController")]
    public void HideController()
    {
        //swap between hand and oculus XRcontroller game objects
        controller.gameObject.SetActive(false);
        hand.gameObject.SetActive(true);
    }

    public void IndicateButtonOnController(OculusControllerType controllerType, OculusButton button)
    {
        if (this.controllerType != controllerType) return;

        ShowController();
        TriggerHapticFeedback();
        PlaceMarkerOverButton(button);
    }

    private void PlaceMarkerOverButton(OculusButton button)
    {
        if (buttonsDict[button].position == null) return;
        buttonMarkerPrefab.transform.position = buttonsDict[button].position;
    }

    private void TriggerHapticFeedback()
    {
        controller.SendHapticImpulse(duration, amplitude);
    }
}
