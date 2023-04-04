using UnityEngine;
using TMPro;

public class FadeText:Fade<TextMeshProUGUI>
{
    [SerializeField] bool _visibleOnStart=true;

    private void Start()
    {
        if (_visibleOnStart) Show();
        else Hide();
    }

    public void Show()
    {
        Color newColor = _fadeColor;
        newColor.a = 1;
        SetAlpha(newColor);
    }

    public void Hide()
    {
        Color newColor = _fadeColor;
        newColor.a = 0;
        SetAlpha(newColor);
    }

    protected override void SetAlpha(Color value) 
    {
        _render.alpha = value.a;
    }

}
