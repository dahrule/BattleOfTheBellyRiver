using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomHandPose : MonoBehaviour
{
    [SerializeField] AnimatorOverrideController RightAnimator;
    [SerializeField] AnimatorOverrideController LeftAnimator;
    [SerializeField] Transform RightAttach;
    [SerializeField] Transform LeftAttach;

    XRBaseInteractable _interactable;
    HandPoseOverride _poseOverride;

    private void Awake()
    {
        _interactable = GetComponent<XRBaseInteractable>();
    }

    private void OnEnable()
    {
        //Add listeners
        if (_interactable != null)
        {
            _interactable.firstHoverEntered.AddListener(SetCustomAttach);

            _interactable.firstSelectEntered.AddListener(SetCustomHandPose);
            _interactable.selectExited.AddListener(UnSetCustomHandPose);
        }
    }

    private void OnDisable()
    {
        //Remove listeners
        _interactable.firstHoverEntered.RemoveListener(SetCustomAttach);

        _interactable.firstSelectEntered.RemoveListener(SetCustomHandPose);
        _interactable.selectExited.RemoveListener(UnSetCustomHandPose);

    }

    private HandPoseOverride GetHandPoseOverride(IXRInteractor interactor)
    {
        GameObject parent = interactor.transform.parent.gameObject;
        return parent.GetComponentInChildren<HandPoseOverride>();
    }

    private void SetCustomAttach(HoverEnterEventArgs arg)
    {
        _poseOverride=GetHandPoseOverride(arg.interactorObject);
        SetAttachPoint(_poseOverride.HandType);
    }

    private void SetCustomHandPose(SelectEnterEventArgs arg)
    {
        _poseOverride = GetHandPoseOverride(arg.interactorObject);
        SetAttachPoint(_poseOverride.HandType);
        SetAnimator(_poseOverride.HandType);
    }
    
    private void UnSetCustomHandPose(SelectExitEventArgs arg)
    {
        _poseOverride = GetHandPoseOverride(arg.interactorObject);
        _poseOverride.RestoreAnimatorOverride();
    }

    private void SetAnimator(HandType hand)
    {
        AnimatorOverrideController animator = (hand ==HandType.Right)
            ? RightAnimator
            : LeftAnimator;

        if (animator == null) return;
        _poseOverride.ApplyAnimatorOverride(animator);
    }

    private void SetAttachPoint(HandType hand)
    {
        Transform attach = (hand == HandType.Right)
            ? RightAttach
            : LeftAttach;
        
        if (attach == null) return;

        XRGrabInteractable grabInteractable = _interactable as XRGrabInteractable;
        grabInteractable.attachTransform=attach;  
    }

}


