using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RotateViewActionComplete : MonoBehaviour
{
    [SerializeField] InputActionReference controllerActionTurn;

    public UnityEvent OnActionComplete;

    void Awake()
    {
        controllerActionTurn.action.performed += TurnPress;
    }
    private void TurnPress(InputAction.CallbackContext obj)
    {
        OnActionComplete?.Invoke();
    }
}
