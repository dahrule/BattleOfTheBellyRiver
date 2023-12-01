using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using TMPro;

public class ObjectScaler : MonoBehaviour
{
    [Tooltip("The input source for scaling the object.")]
    [SerializeField] private InputActionReference scaleAction;
    [Tooltip("The amount to scale the object on each button press.")]
    [SerializeField] private float scaleFactor = 0.1f;
    private Vector3 originalScale;
    private TextMeshPro displayText;
    private bool displayActive = false;
    [Tooltip("The amount of time to display the scaling commands.")]
    [SerializeField] private float displayTime = 3f;
    [Tooltip("The timer for displaying the scaling commands.")]
    [SerializeField] private float displayTimer = 0f;
    private string displayTextIdle = "Press grip button to see commands";
    private string displayTextScaling = "Press thumbstick up/down to scale\nPress thumbstick click to reset";

    void Start()
    {
        originalScale = transform.localScale;
        displayText = GetComponentInChildren<TextMeshPro>();
        if (displayText != null)
        {
            displayText.gameObject.SetActive(false);
        }
        
    }

    void OnEnable()
    {
        if (scaleAction == null) return;
        scaleAction.action.performed += Scale;
    }

    void OnDisable()
    {
        if (scaleAction == null) return;
        scaleAction.action.performed -= Scale;
    }

    void Scale(InputAction.CallbackContext context)
    {
        var thumbstick = context.ReadValue<Vector2>();

        if (thumbstick.y > 0)
        {
            transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
        else if (thumbstick.y < 0)
        {
            transform.localScale -= new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }

        if (context.action.triggered && context.action.name == "reset")
        {
            transform.localScale = originalScale;
        }

        if (context.action.triggered && context.action.name == "display")
        {
            displayActive = true;
            displayTimer = displayTime;
            if (displayText != null)
            {
                displayText.gameObject.SetActive(true);
                displayText.text = displayTextScaling;
            }
        }

        if (!context.action.triggered && context.action.name == "display")
        {
            displayActive = false;
            if (displayText != null)
            {
                displayText.gameObject.SetActive(false);
                displayText.text = displayTextIdle;
            }
        }

        if (displayActive && displayText != null)
        {
            displayTimer -= Time.deltaTime;
            if (displayTimer <= 0f)
            {
                displayActive = false;
                if (displayText != null)
                {
                    displayText.gameObject.SetActive(false);
                    displayText.text = displayTextIdle;
                }
            }
        }
        else if (!displayActive && context.action.ReadValue<float>() == 1f)
        {
            if (displayText != null)
            {
                displayText.gameObject.SetActive(true);
                displayText.text = "Object is still being held";
            }
        }
    }
}
