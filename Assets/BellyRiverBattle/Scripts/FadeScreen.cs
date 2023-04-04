using UnityEngine;

public class FadeScreen : Fade<Renderer>
{
    [SerializeField] bool _fadeOnStart = true;
    void Start()
    {
        if (_fadeOnStart)
            FadeIn();
    }
    protected override void SetAlpha(Color value)
    {
        _render.material.SetColor("_BaseColor", value);
    }
}
