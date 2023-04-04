using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine;


/// <summary>
/// Hides the teleport ray at start and shows it only when moving the joystick forward
/// </summary>
public class HandController : MonoBehaviour
{
    [SerializeField]
    GameObject baseControllerObject;
    [SerializeField]
    GameObject teleportGameObject;
    [SerializeField]
    InputActionReference teleportActivationReference;

    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;

    // Start is called before the first frame update
    void Start()
    {
        teleportActivationReference.action.performed += TeleportModeActivate;
        teleportActivationReference.action.canceled += TeleportModeCancel;
    }

    private void TeleportModeCancel(InputAction.CallbackContext obj)
    {
        Invoke("DeactivateTeleporter", 0.1f);
    }
    private void TeleportModeActivate(InputAction.CallbackContext obj)
    {
        onTeleportActivate.Invoke();
    }
    void DeactivateTeleporter()
    {
        onTeleportCancel.Invoke();
    }
   
}
