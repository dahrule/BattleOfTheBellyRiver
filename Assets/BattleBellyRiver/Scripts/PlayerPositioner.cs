using UnityEngine;

public class PlayerPositioner : MonoBehaviour
{
    [SerializeField] public Transform _resetTransform;
    [SerializeField] GameObject _player;
    [SerializeField] Camera _playerHead;

    private void Start()
    {
        ResetPosition();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) ResetPosition(); //TO TEST
    }

    [ContextMenu("Reset Position")]
    public void ResetPosition()
    {
        if (_player == null || _playerHead == null) return;

        if (_resetTransform == null) _resetTransform = _player.transform;

        var rotationAngleY = _resetTransform.rotation.eulerAngles.y - 
                _playerHead.transform.rotation.eulerAngles.y;
        _player.transform.Rotate(0,rotationAngleY,0);

        var distanceDiff = _resetTransform.position - _playerHead.transform.position;
        _player.transform.position += distanceDiff;
    }
}
