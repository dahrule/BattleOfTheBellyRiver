using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class TextPrompt : MonoBehaviour
{
    [Header("TextPrompt properties")]
    [SerializeField] protected bool _enabledAtStart = false;
    [SerializeField] protected AudioClip _displaySFX;
    [SerializeField] protected TextMeshProUGUI _TextObject;

    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    protected virtual void Start()
    {
        if (_enabledAtStart) Display();
        else Hide();
    }
    public virtual void Display()
    {
        this.gameObject.SetActive(true);
        PlaySFX(_displaySFX);
    }
    public virtual void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void PlaySFX(AudioClip sfx)
    {
        if (sfx == null) return;
        _audioSource.PlayOneShot(sfx);
    }
}
