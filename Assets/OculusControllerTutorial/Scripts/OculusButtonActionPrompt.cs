using UnityEngine;
using System.Collections.Generic;

public enum OculusControllerType
{
    Left, Right, Any
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
public class OculusButtonActionPrompt : TextPrompt
{   
    enum ButtonAction { Press, PressHold, PressTwice, Use }

    [Header("OculusButtonAction properties")]
    [SerializeField] OculusControllerType controllerType;
    [SerializeField] OculusButton button;
    [SerializeField] ButtonAction action;
    [Tooltip("Text describing the result of interacting with a button.")]
    [SerializeField] string consequence;
    [Tooltip("The controller this prompt is refering. Must contain XRController and OculusButtonHighlighter components")]
    [SerializeField] OculusButtonHighlighter[] controllers;

    void CreateMessage()
    {
        if (_TextObject == null) return;

        // Determine the button mapping to use based on the controller type
        Dictionary<OculusButton, string> buttonMapping = GetButtonMapping(controllerType);

        // Get the button name to display based on the button type.
        string buttonName = buttonMapping.ContainsKey(button) ? buttonMapping[button] : button.ToString();

        //Add the word "button" for all cases except the Thumbstick case.
        string buttonWord = button==OculusButton.Thumbstick ? "" : "button";

        //Add the controller type name "button" for all cases except the Thumbstick case.
        string controllerWord = controllerType == OculusControllerType.Any ? "" : $"on {controllerType} controller";

        // Build the message
        string message = $"{action} the {buttonName} {buttonWord} {controllerWord} to {consequence}";
        _TextObject.text = message;
    }

    Dictionary<OculusButton, string> GetButtonMapping(OculusControllerType controllerType)
    {
        //the controller is on the left
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
        else //the controller is on the right
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
        foreach (var controller in controllers)
        {
            if (controller != null)
            {
                controller.Hide();
            }
        }
        base.Hide();
    }

    public override void Display()
    {
        base.Display();
        CreateMessage();
        foreach (var controller in controllers)
        {
            if (controller != null)
            {
                controller.IndicateButtonOnController(controllerType, button);
            }
        }
    }
}
