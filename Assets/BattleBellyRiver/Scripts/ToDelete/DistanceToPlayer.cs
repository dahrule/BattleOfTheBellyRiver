using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DistanceToPlayer : MonoBehaviour
{
    [SerializeField] Camera _playerView;
    [SerializeField] float horizontalOffset = 2f;
    [SerializeField] float verticalOffset = 0f;

    private void Awake()
    {
        //if (_player == null) _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        float vertical = _playerView.transform.position.y + verticalOffset;
        float horizontal = _playerView.transform.position.y + horizontalOffset;
        Vector3 newPosition = new Vector3(transform.position.x, vertical, transform.position.z);
        transform.position = newPosition;
        
        
    }

    private void OnDrawGizmos()
    {
        
    }

}
