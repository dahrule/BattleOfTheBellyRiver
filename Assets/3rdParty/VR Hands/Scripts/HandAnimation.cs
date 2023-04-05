using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimation : MonoBehaviour
{
    [SerializeField]
    InputActionReference controllerActionGrip;
    [SerializeField]
    InputActionReference controllerActionTrigger;
    Animator handAnimator;

    private void Awake()
    {
        controllerActionGrip.action.performed += GripPress;
        controllerActionTrigger.action.performed += TriggerPress;

        handAnimator = GetComponent<Animator>();
    }

    private void TriggerPress(InputAction.CallbackContext obj)
    {
        if (handAnimator == null) return;
        handAnimator.SetFloat("Trigger",obj.ReadValue<float>());
    }

    private void GripPress(InputAction.CallbackContext obj)
    {
        if (handAnimator == null) return;
        handAnimator.SetFloat("Grip", obj.ReadValue<float>());
    }
}
