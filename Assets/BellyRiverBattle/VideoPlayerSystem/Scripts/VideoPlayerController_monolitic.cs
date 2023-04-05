using UnityEngine;
using UnityEngine.Video;
using TMPro;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(VideoPlayer))] 
public class VideoPlayerController_monolitic : MonoBehaviour
{
    [SerializeField] Sprite pauseButtonSprite;
    [SerializeField] Sprite playButtonSprite;
    [SerializeField] Image PlayPauseButton;
    [SerializeField] TextMeshProUGUI timeCountUI;
    [SerializeField] TextMeshProUGUI clipNameUI;
    [SerializeField] Scrollbar timeSliderUI;
    [SerializeField] VideoClip[] videoClips;

    VideoPlayer videoPlayer;
    int videoClipIndex;
    IEnumerator timeCount;
   
    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        timeCount = TimeCountRoutine(); 
        videoPlayer.loopPointReached += (VideoPlayer) => SetNextClip(); //handles when playing video ends.
    }
    void Start()
    {
        videoPlayer.targetTexture.Release(); //Last frame doesnot persist between sessions.

        if (videoClips.Length != 0) videoPlayer.clip = videoClips[0];
        
        UpdateUI();
    }
    [ContextMenu("PlayPause")]
    public void PlayPause()
    {
        if (videoPlayer.clip == null) return;

        if (videoPlayer.isPlaying)
        {
            //Pause
            videoPlayer.Pause();
            if(playButtonSprite!=null) PlayPauseButton.sprite = playButtonSprite; //Change button UI

            if(timeCount!=null) StopCoroutine(timeCount);
        }else
        {
            //Play
            videoPlayer.Play();
            if (pauseButtonSprite != null) PlayPauseButton.sprite = pauseButtonSprite; //Change button UI

            if (timeCount != null)
            {
                StopCoroutine(timeCount);
                StartCoroutine(timeCount);
            }
        }
    }
    [ContextMenu("SetNextClip")]
    public void SetNextClip()
    {
        if (videoClips.Length <= 1) return;

        //Update index
        videoClipIndex++;
        if (videoClipIndex >= videoClips.Length) videoClipIndex = 0;

        videoPlayer.clip = videoClips[videoClipIndex];
        SetClipNameUI();
        PlayPause();
    }
    [ContextMenu("SetPreviousClip")]
    public void SetPreviousClip()
    {
        if (videoClips.Length <= 1) return;

        //Update index
        videoClipIndex--;
        if (videoClipIndex <0) videoClipIndex = videoClips.Length - 1;

        videoPlayer.clip = videoClips[videoClipIndex];
        SetClipNameUI();
        PlayPause();
    }
    string GetClipTime_MinSec()
    {
        VideoClip clip = videoPlayer.clip;
        if (clip == null) return "00";

        string minutes = Mathf.Floor((int)videoPlayer.time / 60).ToString("00");
        string seconds = ((int)videoPlayer.time % 60).ToString("00");

        return minutes +":"+seconds;
    }
    string GetTotalClipTime_MinSec()
    {
        VideoClip clip = videoPlayer.clip;
        if (clip == null) return "00";

        string minutes = Mathf.Floor((int)videoPlayer.clip.length / 60).ToString("00");
        string seconds = ((int)videoPlayer.clip.length % 60).ToString("00");

        return minutes + ":" + seconds;
    }
    double CalculatePlayedFraction()
    {
        VideoClip clip = videoPlayer.clip;
        if (clip == null) return 0;

        double fraction = (double)videoPlayer.frame / (double)videoPlayer.clip.frameCount;
        return fraction;
    }
    void UpdateUI()
    {
        SetTimeUI();
        SetClipNameUI();
    }
    private void SetTimeUI()
    {
        //Update timeUI
        timeCountUI.text = GetClipTime_MinSec() + " / " + GetTotalClipTime_MinSec();
        timeSliderUI.size = (float)CalculatePlayedFraction();
    }
    void SetClipNameUI()
    {
        VideoClip clip = videoPlayer.clip;
        if (clip == null) clipNameUI.text = "No video assigned";
        else clipNameUI.text = videoPlayer.clip.name;
    }
    IEnumerator TimeCountRoutine()
    {
        //Call every 1 second
        WaitForSeconds waitTime = new WaitForSeconds(1.0f);
        while (true)
        {
            SetTimeUI();
            yield return waitTime;
        }
    }
}
