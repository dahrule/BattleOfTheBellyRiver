using UnityEngine;
using UnityEngine.Playables;

public class TimelinesController : MonoBehaviour
{
    [SerializeField] PlayableDirector smudgeMixCompleteTimeline;
    //Add References to other Playable directors here.

    private void Start()
    {
        SubscribetoSceneEvents();
    }

    private void SubscribetoSceneEvents()
    {
        SmudgeBox.OnMixComplete += PlaySmudgeMixCompleteCinematic;
    }

    private void PlaySmudgeMixCompleteCinematic()
    {
        smudgeMixCompleteTimeline.Play();
        SmudgeBox.OnMixComplete -= PlaySmudgeMixCompleteCinematic;
    }

}

