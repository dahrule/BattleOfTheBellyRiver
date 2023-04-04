using UnityEngine;

public class CombatTarget : MonoBehaviour
{
    public AnimatorOverrideController _attackOverride;
    [SerializeField] AudioClip _shotClip;
    AudioSource _audioSource;
    Animator _animController;
    readonly int _shootHash = Animator.StringToHash("shoot");
    LookAtPlayer _lookAtPlayer;

    private void Awake()
    { 
        _audioSource = GetComponent<AudioSource>();
        _animController = GetComponent<Animator>();
        _lookAtPlayer= GetComponent<LookAtPlayer>();
    }

    //Call from animation event
    public void PlayGunShot()
    {
        if (_audioSource != null && _shotClip != null)
            _audioSource.PlayOneShot(_shotClip);
    }

    public void FireGun()
    {
        if(_lookAtPlayer!=null)
            _lookAtPlayer.enabled = true;
        if (_animController != null)
            _animController.SetTrigger(_shootHash);
    }
}
