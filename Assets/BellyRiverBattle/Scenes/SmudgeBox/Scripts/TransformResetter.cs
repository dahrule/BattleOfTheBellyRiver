using UnityEngine;

public class TransformResetter : MonoBehaviour
{
    public Transform TransformAtStart { private set; get; }
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    void Start()
    {
        // Store the initial transform values.
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;

        // Create a new Transform with the initial values.
        TransformAtStart = new GameObject("TransformAtStart").transform;
        TransformAtStart.SetPositionAndRotation(initialPosition, initialRotation);
        TransformAtStart.localScale = initialScale;
    }

    [ContextMenu("ResetTransform")]
    public void ResetTransform()
    {
        this.transform.SetPositionAndRotation(initialPosition, initialRotation);
        this.transform.localScale = initialScale;
    }
}
