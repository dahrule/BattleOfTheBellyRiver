using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TextPrompt : MonoBehaviour
{
    [Header("TextPrompt properties")]
    [SerializeField] protected bool enabledAtStart = false;
    [SerializeField] protected AudioClip popUpSfx;
    [SerializeField] protected AudioClip actionCompleteSfx;
    [SerializeField] protected float _secondsToReappear = 3 * 60;
    [SerializeField] protected TextMeshProUGUI TextObject;

    Vector3 _position;
    IEnumerator _timeCount;
    public FadeText fadeTextComponent;
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if(TextObject!=null)
            fadeTextComponent = TextObject.GetComponent<FadeText>();
    }
    public virtual void Display()
    {
        this.gameObject.SetActive(true);
        PopUp();
        _timeCount = TimeCountRoutine();
        StartCoroutine(_timeCount);
    }
    public virtual void Hide()
    {
        if (_timeCount != null)
            StopCoroutine(_timeCount);
        this.gameObject.SetActive(false);
    }

    [ContextMenu("PopUp")]
    public void PopUp()
    {
        if (TextObject == null) return;

        if (fadeTextComponent != null) fadeTextComponent.Show();
        if (popUpSfx != null) _audioSource.PlayOneShot(popUpSfx); //Audio signal to indicate that the text has appeared.
    }

    [ContextMenu("Fade")]
    public void Fade()
    {
        if (fadeTextComponent == null) return;

        //Assumes that _secondsToReapper is significantly longer than the text fading time.
        //If not the case, sync issues may appear between the sound sfx and the visibility of text.
        //Alternatively, use a callback to the OnFadeComplete events of the Fade class.
        if (_timeCount != null)
        {
            StopCoroutine(_timeCount);
            StartCoroutine(_timeCount);
        }

        fadeTextComponent.FadeIn();
    }
    IEnumerator TimeCountRoutine()
    {
        float count = 0;
        WaitForSeconds afterOneSecond = new(1f);
        while (true)
        {
            yield return afterOneSecond;
            count++;
            if (count > _secondsToReappear)
            {
                count = 0;
                PopUp();
            }
        }
    }

    public void PlayActionCompleteSound()
    {
        _audioSource.PlayOneShot(actionCompleteSfx);
    }
}
