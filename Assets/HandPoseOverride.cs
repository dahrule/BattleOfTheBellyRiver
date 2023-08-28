using UnityEngine;

public enum HandType { Left, Right }

[RequireComponent(typeof(Animator))]
public class HandPoseOverride : MonoBehaviour
{
    [field:SerializeField] public HandType HandType { get; private set; }

    Animator _animator;
    RuntimeAnimatorController _originalController;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        // Cache the original Animator Controller
        _originalController = _animator.runtimeAnimatorController;
    }

    //Called from CustomHandPose class in an Interactable
    public void ApplyAnimatorOverride(AnimatorOverrideController overrideController)
    {
        _animator.runtimeAnimatorController = overrideController;
    }

    //Called from CustomHandPose class in an Interactable
    public void RestoreAnimatorOverride()
    {
        _animator.runtimeAnimatorController = _originalController;
    }
}
