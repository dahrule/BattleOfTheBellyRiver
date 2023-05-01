using UnityEngine;
using TMPro;
using System.Collections.Generic;

public enum OculusControllerType
{
    Left, Right
}
public enum OculusButton
{
    Primary,
    Secondary,
    Trigger,
    Grip,
    Thumbstick,
    ThumbstickButton,
    System
}

/// <summary>
///Creates a message for a specified action's corresponding button, and highlights the button on a Controller, if one is given.
/// </summary>
public class OculusButtonActionPrompt : ActionPrompt
{   
    enum ButtonAction { Press, PressHold, PressTwice, Use }

    [SerializeField] OculusControllerType controllerType;
    [SerializeField] OculusButton button;
    [SerializeField] ButtonAction action;

    [Tooltip("Text describing the result of interacting with a button.")]
    [SerializeField] string consequence;

    [SerializeField] TextMeshProUGUI TextObject;
    [SerializeField] OculusButtonHighlighter controllerHighlighter;

    void Start()
    {
        if (enabledAtStart) Display();
        else Hide();
    }

    void CreateMessage()
    {
        // Determine the button mapping to use based on the controller type
        Dictionary<OculusButton, string> buttonMapping = GetButtonMapping(controllerType);

        // Get the button name to display based on the button type.
        string buttonName = buttonMapping.ContainsKey(button) ? buttonMapping[button] : button.ToString();

        //Add the word "button" for all cases except the Thumbstick case.
        string buttonWord = button==OculusButton.Thumbstick ? "" : "button";

        // Build the message string
        string message = $"{action} the {buttonName} {buttonWord} on the {controllerType} controller to {consequence}";
        TextObject.text = message;
    }

    Dictionary<OculusButton, string> GetButtonMapping(OculusControllerType controllerType)
    {
        if (controllerType == OculusControllerType.Left)
        {
            return new Dictionary<OculusButton, string>
        {
            { OculusButton.Primary, "Y" },
            { OculusButton.Secondary, "X" },
            { OculusButton.ThumbstickButton, "Thumbstick" },
            // Add more button mappings as needed
        };
        }
        else
        {
            return new Dictionary<OculusButton, string>
        {
            { OculusButton.Primary, "A" },
            { OculusButton.Secondary, "B" },
            { OculusButton.ThumbstickButton, "Thumbstick" },
            // Add more button mappings as needed
        };
        }
    }

    public override void Hide()
    {
        controllerHighlighter.HideController();
        base.Hide();
    }

    public override void Display()
    {
        base.Display();
        CreateMessage();
        if (controllerHighlighter != null)
            controllerHighlighter.IndicateButtonOnController(controllerType, button);
    }
}
