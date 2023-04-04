using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvetTrigger : MonoBehaviour
{
    [SerializeField] string activator;
    [SerializeField] FadeText fadeText;

    UnityEvent onTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (activator == null) return;

        if(other.CompareTag(activator))
        {
            onTriggerEnter?.Invoke();
        }
    }
}
