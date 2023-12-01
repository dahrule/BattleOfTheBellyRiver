using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public abstract class Fade<T> : MonoBehaviour
{
    [SerializeField] protected Color _fadeColor;
    [SerializeField] public float _fadeDuration = 2;

    [SerializeField] UnityEvent OnFadeInComplete;
    [SerializeField] UnityEvent OnFadeOutComplete;
    [SerializeField] UnityEvent OnFadePercentageComplete; //part of experimental

    [SerializeField] float _durationPercentage = 0.95f; //part of experimental

    protected T _render;
    bool _fadeRoutineRunning = false;
    [SerializeField] float _marginOfError=0.001f; //part of experimental

    public bool FadeRuotineRunnig { private set { _fadeRoutineRunning = value;} get { return _fadeRoutineRunning;} }


    protected virtual void Awake()
    {
        _render = GetComponent<T>();
    }

    public virtual void FadeIn()
    {
        FadeEffect(1, 0);
    }

    public virtual void FadeOut()
    {
        FadeEffect(0, 1);
    }

    private void FadeEffect(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        FadeRuotineRunnig = true;

        Color newColor = _fadeColor;

        float timer = 0.0f;
        while (timer <= _fadeDuration)
        {
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / _fadeDuration);

            //parf of experimental
            if (Aproximates(_fadeDuration*_durationPercentage,timer,_marginOfError))
            {
                OnFadePercentageComplete?.Invoke();
            }

            SetAlpha(newColor);
            timer += Time.deltaTime;

            yield return null;
        }

        //Ensure alpha out is set as the final alpha value.
        newColor.a = alphaOut;
        SetAlpha(newColor);

        //Broadcast when fadein or fadeout is completed.
        if (alphaIn == 0) OnFadeOutComplete?.Invoke();
        else OnFadeInComplete?.Invoke();

        FadeRuotineRunnig = false;
    }

    private bool Aproximates(float a, float b, float tolerance)
    {
        return Mathf.Abs(a-b) <= tolerance;
    }

    protected abstract void SetAlpha(Color value);
}


