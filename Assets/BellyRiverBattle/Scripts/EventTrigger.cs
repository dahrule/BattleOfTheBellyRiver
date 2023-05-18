using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    [Tooltip("object tags that activate this trigger")]
    [SerializeField] string[] activatorTags;
    [SerializeField] int numberOfEventCanTrigger = 1;
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;

    int counter=0;

    private void OnTriggerEnter(Collider other)
    {
        if (activatorTags == null) return;

        if (counter >= numberOfEventCanTrigger) this.gameObject.SetActive(false);
  
        else if(IsActivatorColliding(other))
        {
            onTriggerEnter?.Invoke();
            counter++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (activatorTags == null) return;


        if (IsActivatorColliding(other))
        {
            onTriggerExit?.Invoke();
        }
    }

    private bool IsActivatorColliding (Collider other)
    {
        GameObject activator = other.gameObject;
        foreach (var tag in activatorTags)
        {
             if(activator.CompareTag(tag)) return true;
        }
        return false;
    }
}
