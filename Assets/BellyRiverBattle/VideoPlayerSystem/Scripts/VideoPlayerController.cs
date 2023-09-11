using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerController : MonoBehaviour
{
    [SerializeField] VideoClip[] _videoClips;

    VideoPlayer _videoPlayer;
    int _videoClipIndex;
    IEnumerator _timeCount;
    [SerializeField] string _nextScene;

    //[SerializeField] GameObject SceneManager;
    

    public event Action OnPause;
    public event Action OnPlay;
    public event Action<VideoClip> OnClipChange;
    public event Action OnTimeCountUpdate;

    private void Awake()
    {       
        _videoPlayer = GetComponent<VideoPlayer>();
        _timeCount = TimeCountRoutine();
        _videoPlayer.loopPointReached += (VideoPlayer) => SetNextClip(); //handles when playing video ends.        
        
    }

    void Start()
    {
        //SceneManager = GameObject.Find("SceneManager"); //finds SceneManager if not expressly filled
        _videoPlayer.targetTexture.Release(); //Last frame doesnot persist between sessions.

        if (_videoClips.Length != 0) _videoPlayer.clip = _videoClips[0];
        OnClipChange?.Invoke(_videoPlayer.clip);
    }

    [ContextMenu("PlayPause")]
    public void PlayPause()
    {
        if (_videoPlayer.clip == null) return;

        if (_videoPlayer.isPlaying)
        {
            //Pause
            _videoPlayer.Pause();
            
            if (_timeCount != null) StopCoroutine(_timeCount);

            OnPause?.Invoke();
        }
        else
        {
            //Play
            _videoPlayer.Play();

            if (_timeCount != null)
            {
                StopCoroutine(_timeCount);
                StartCoroutine(_timeCount);
            }

            OnPlay?.Invoke();
        }
    }

    [ContextMenu("SetNextClip")]
    public void SetNextClip()
    {
        if (_videoClips.Length <= 1) return;

        //Update index
        _videoClipIndex++;
        if (_videoClipIndex >= _videoClips.Length)
        {
            _videoClipIndex= 0;
            SceneManager.LoadScene(_nextScene);
            //gameObject.SendMessage("FadeOutScene");
            _videoPlayer.Stop();// Stop playback when all videos have been played     
                        
            return;
        }

        _videoPlayer.clip = _videoClips[_videoClipIndex];

        OnClipChange?.Invoke(_videoPlayer.clip);
       
        PlayPause();
    }

    [ContextMenu("SetPreviousClip")]
    public void SetPreviousClip()
    {
        if (_videoClips.Length <= 1) return;

        //Update index
        _videoClipIndex--;
        if (_videoClipIndex < 0) _videoClipIndex = _videoClips.Length - 1;

        _videoPlayer.clip = _videoClips[_videoClipIndex];

        OnClipChange?.Invoke(_videoPlayer.clip);

        PlayPause();
    }

    public string GetClipTime_MinSec()
    {
        VideoClip clip = _videoPlayer.clip;
        if (clip == null) return "00";

        string minutes = Mathf.Floor((int)_videoPlayer.time / 60).ToString("00");
        string seconds = ((int)_videoPlayer.time % 60).ToString("00");

        return minutes + ":" + seconds;
    }

    public string GetTotalClipTime_MinSec()
    {
        VideoClip clip = _videoPlayer.clip;
        if (clip == null) return "00";

        string minutes = Mathf.Floor((int)_videoPlayer.clip.length / 60).ToString("00");
        string seconds = ((int)_videoPlayer.clip.length % 60).ToString("00");

        return minutes + ":" + seconds;
    }

    public double CalculatePlayedFraction()
    {
        VideoClip clip = _videoPlayer.clip;
        if (clip == null) return 0;

        double fraction = (double)_videoPlayer.frame / (double)_videoPlayer.clip.frameCount;
        return fraction;
    }

    IEnumerator TimeCountRoutine()
    {
        //Call every 1 second
        WaitForSeconds waitTime = new WaitForSeconds(1.0f); 
        while (true)
        {
            OnTimeCountUpdate?.Invoke();
            yield return waitTime;
        }
    }
    
}
