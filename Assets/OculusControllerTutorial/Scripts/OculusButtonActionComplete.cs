using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OculusButtonActionComplete : MonoBehaviour
{
    [SerializeField] InputActionReference controllerAction;
    [SerializeField] OculusButtonActionPrompt actionPrompt;

    public UnityEvent OnActionComplete;

    void OnEnable()
    {
        controllerAction.action.performed += ButtonPress;
    }

    void OnDisable()
    {
        controllerAction.action.performed -= ButtonPress;
    }
    private void ButtonPress(InputAction.CallbackContext obj)
    {
        if (OnActionComplete == null) return;

        OnActionComplete?.Invoke();
        this.gameObject.SetActive(false);
    }
}
