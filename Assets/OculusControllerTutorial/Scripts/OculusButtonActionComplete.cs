using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEditor.Events;

public class OculusButtonActionComplete : MonoBehaviour
{
    [SerializeField] InputActionReference controllerAction;
    [SerializeField] OculusButtonActionPrompt actionPrompt;

    public UnityEvent OnActionComplete;

    void OnEnable()
    {
        if (controllerAction == null) return;
        controllerAction.action.performed += ButtonPress;
    }

    void OnDisable()
    {
        if (OnActionComplete != null)
        {
            UnityEventBase eventBase = OnActionComplete;
            int persistentListenerCount = eventBase.GetPersistentEventCount();
            for (int i = 0; i < persistentListenerCount; i++)
            {
                eventBase.SetPersistentListenerState(i, UnityEventCallState.Off);
            }
        }

        if (controllerAction != null)
            controllerAction.action.performed -= ButtonPress;
    }
    private void ButtonPress(InputAction.CallbackContext obj)
    {
        OnActionComplete?.Invoke();
        this.gameObject.SetActive(false);
    }

    public void CallOnActionCompleteEvent()
    {
        OnActionComplete?.Invoke();
        this.gameObject.SetActive(false);
        
    }
}
