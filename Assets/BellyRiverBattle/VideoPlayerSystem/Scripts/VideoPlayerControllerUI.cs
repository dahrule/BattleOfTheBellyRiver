using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerControllerUI : MonoBehaviour
{
    [SerializeField] VideoPlayerController _videoplayerController;

    [Header("UI")]
    [SerializeField] Sprite _pauseButtonSprite;
    [SerializeField] Sprite _playButtonSprite;
    [SerializeField] Image _PlayPauseButton;
    [SerializeField] TextMeshProUGUI _timeCountText;
    [SerializeField] TextMeshProUGUI _clipNameText;
    [SerializeField] Scrollbar _timeSlider;

   
    private void OnEnable()
    {
        _videoplayerController.OnPlay += SetPauseButtonImage;
        _videoplayerController.OnPause += SetPlayButtonImage;
        _videoplayerController.OnClipChange += SetClipNameText;
        _videoplayerController.OnClipChange += (VideoClip)=>UpdateTimeUI();
        _videoplayerController.OnTimeCountUpdate += UpdateTimeUI;
    }

    private void OnDisable()
    {
        _videoplayerController.OnPlay -= SetPauseButtonImage;
        _videoplayerController.OnPause -= SetPlayButtonImage;
        _videoplayerController.OnClipChange -= SetClipNameText;
        _videoplayerController.OnClipChange -= (VideoClip) => UpdateTimeUI();
        _videoplayerController.OnTimeCountUpdate -= UpdateTimeUI;
    }

    void SetPlayButtonImage()
    {
        if (_playButtonSprite == null) return;
        _PlayPauseButton.sprite = _playButtonSprite;
    }

    void SetPauseButtonImage()
    {
        if (_pauseButtonSprite == null) return;
        _PlayPauseButton.sprite = _pauseButtonSprite;
    }

    private void UpdateTimeUI()
    {
        if (_timeCountText == null || _timeSlider == null) return;

        _timeCountText.text = _videoplayerController.GetClipTime_MinSec() + " / " + _videoplayerController.GetTotalClipTime_MinSec();
        _timeSlider.size = (float)_videoplayerController.CalculatePlayedFraction();
    }

    void SetClipNameText(VideoClip clip)
    {
        if (_clipNameText == null) return;

        if (clip == null) _clipNameText.text = "No video assigned";
        else _clipNameText.text = clip.name;
    }
}
