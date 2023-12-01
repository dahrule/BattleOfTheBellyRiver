using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [field:SerializeField] public Transform Target { get; set; }

    void Update()
    {
        if (Target == null) return;
        
        transform.position = Target.position;
    }
}
