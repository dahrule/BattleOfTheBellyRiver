using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CustomHandPoseAnimator : MonoBehaviour
{
    public enum HandType { Left, Right }

    public HandType _handType;
    RuntimeAnimatorController _defaultAnimController;
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        // Cache the original Animator Controller
        _defaultAnimController = _animator.runtimeAnimatorController;
    }

    //Called from CustomHandPoseHandler class in an Interactable
    public void SetPose(AnimatorOverrideController newAnimationController)
    {
        _animator.runtimeAnimatorController = newAnimationController;
    }

    //Called from CustomHandPoseHandler class in an Interactable
    public void UnSetPose()
    {
        _animator.runtimeAnimatorController = _defaultAnimController;
    }
}
