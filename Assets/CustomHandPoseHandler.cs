using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomHandPoseHandler : MonoBehaviour
{
    XRBaseInteractable _interactable;
    CustomHandPoseAnimator _customHandPose;
   [SerializeField] Artefact artefact;

    private void Awake()
    {
        _interactable = GetComponent<XRBaseInteractable>();
    }

    private void OnEnable()
    {
        if (_interactable != null)
        {
            _interactable.selectEntered.AddListener(SetCustomHandPose);
            _interactable.selectExited.AddListener(UnsetCustomHandPose);
        }
    }

    private void OnDisable()
    {
        if (_interactable != null)
        {
            _interactable.selectEntered.RemoveListener(SetCustomHandPose);
            _interactable.selectExited.RemoveListener(UnsetCustomHandPose);
        }
    }

    private void SetCustomHandPose(SelectEnterEventArgs arg)
    {
        GameObject parent = arg.interactorObject.transform.parent.gameObject;
        _customHandPose = parent.GetComponentInChildren<CustomHandPoseAnimator>();

        AnimatorOverrideController animOverrideController=(_customHandPose._handType == CustomHandPoseAnimator.HandType.Right) ? artefact.rightHand_overrideController:artefact.leftHand_overrideController;
        _customHandPose.SetPose(animOverrideController);

        SetAttachPoint();

        /*//set attach point
        XRGrabInteractable grabInteractable = _interactable as XRGrabInteractable;
        if (grabInteractable != null)
        {
            grabInteractable.attachTransform = (_customHandPose._handType == CustomHandPoseAnimator.HandModelType.Right) ? artefact.rightAttachPoint : artefact.leftAttachPoint;
        }*/

    }

    private void UnsetCustomHandPose(SelectExitEventArgs arg)
    {
        _customHandPose.UnSetPose();
    }

    void SetAttachPoint()
    {
        //set attach point
        XRGrabInteractable grabInteractable = _interactable as XRGrabInteractable;
        // Original object
        Transform originalObject = grabInteractable.attachTransform;

        // Create the mirror object
        GameObject mirrorObject = new GameObject("MirrorTransfom");

        // Set the position and rotation of the mirror object to match the original object
        mirrorObject.transform.position = originalObject.position;
        mirrorObject.transform.rotation = originalObject.rotation;

        // Invert the scale along the X-axis to mirror the object
        Vector3 scale = mirrorObject.transform.localScale;
        scale.x *= -1;
        mirrorObject.transform.localScale = scale;
        grabInteractable.attachTransform = mirrorObject.transform;
    }
}

