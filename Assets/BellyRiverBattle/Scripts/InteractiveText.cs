using System.Collections;
using UnityEngine;
using TS.GazeInteraction;

[RequireComponent(typeof(AudioSource))]
public class InteractiveText : MonoBehaviour
{
    [SerializeField] float _secondsToReappear=5*60; //5 min
    [SerializeField] AudioClip _audioClip;
    [SerializeField] FadeText _fadeText;
    [SerializeField] GazeInteractable _gazeInteractable;

    Vector3 _position;
    IEnumerator _timeCount;

    private void Awake()
    {
        _position = this.transform.position;
    }

    private void Start()
    {
        PopUp();

        _timeCount = TimeCountRoutine();
        StartCoroutine(_timeCount);
    }

    [ContextMenu("DisableObject")]
    public void DisableInteractiveText()
    {
        if(_timeCount!=null) 
            StopCoroutine(_timeCount);
        this.gameObject.SetActive(false);
    }

    [ContextMenu("PopUp")]
    public void PopUp()
    {
        if (_fadeText == null) return;

        _fadeText.Show();
        if (_audioClip != null) AudioSource.PlayClipAtPoint(_audioClip, _position); //Play feedback 
        if (_gazeInteractable != null) _gazeInteractable.Enable(true);
    }

    [ContextMenu("Fade")]
    public void Fade()
    {
        if (_fadeText == null) return;

        if (_gazeInteractable != null) _gazeInteractable.Enable(false);

        //Assumes that _secondsToReapper is significantly longer than the text fading time.
        //If not the case, sync issues may appear between the sound sfx and the visibility of text.
        //Alternatively, use a callback to the OnFadeComplete events of the Fade class.
        if(_timeCount!=null)
        {
            StopCoroutine(_timeCount);
            StartCoroutine(_timeCount);
        }
       
        _fadeText.FadeIn();
    }
    IEnumerator TimeCountRoutine()
    {
        float count = 0;
        WaitForSeconds afterOneSecond = new(1f);
        while(true)
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
}
