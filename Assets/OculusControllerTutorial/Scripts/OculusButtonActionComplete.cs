using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OculusButtonActionComplete : MonoBehaviour
{
    [SerializeField] InputActionReference controllerAction;

    public UnityEvent OnActionComplete;

    void Awake()
    {
        controllerAction.action.performed += ButtonPress;
    }
    private void ButtonPress(InputAction.CallbackContext obj)
    {
        OnActionComplete?.Invoke();
    }
}
