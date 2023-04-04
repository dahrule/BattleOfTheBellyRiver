
using UnityEngine;

public class Logger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] bool _showLogs;
    [SerializeField] Color _prefixColor;
    [SerializeField] string _prefix;

    string _hexColor;

    public void Log(object message, Object sender)
    {
        if (!_showLogs) return;

        Debug.Log($"<color={_hexColor}>{_prefix}: {message}</color>", sender);
    }
    private void OnValidate()
    {
        _hexColor = "#" + ColorUtility.ToHtmlStringRGB(_prefixColor);
    }

}
