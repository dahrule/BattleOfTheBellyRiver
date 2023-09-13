using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomizeHandPose1 : MonoBehaviour
{
    public enum HandModelType { Left, Right }

    public HandModelType handType;
    RuntimeAnimatorController _defaultAnimController;
    XRGrabInteractable _interactable;
    [SerializeField] Artefact artefact;

    void Awake()
    {
        _interactable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        _interactable.selectEntered.AddListener(SetPose);
        _interactable.selectExited.AddListener(UnSetPose);
    }
    private void OnDisable()
    {
        _interactable.selectEntered.RemoveListener(SetPose);
        _interactable.selectExited.RemoveListener(UnSetPose);
    }

    public void SetPose(BaseInteractionEventArgs arg)
    {

        if (arg.interactorObject is XRBaseInteractor)
        {
            GameObject parent = arg.interactorObject.transform.parent.gameObject;
            Animator animator = parent.GetComponentInChildren<Animator>();

            if (animator == null) return;
            
            // Cache the original Animator Controller
            _defaultAnimController = animator.runtimeAnimatorController;

            // Assign the correct Animator Override Controller.
            animator.runtimeAnimatorController = handType==HandModelType.Right ? artefact.rightHand_overrideController : artefact.leftHand_overrideController;
        }
    }

    public void UnSetPose(BaseInteractionEventArgs arg)
    {

        GameObject parent = arg.interactorObject.transform.parent.gameObject;
        Animator animator = parent.GetComponentInChildren<Animator>();

        if (animator == null) return;
        
        animator.runtimeAnimatorController = _defaultAnimController;
        
    }
}
    


