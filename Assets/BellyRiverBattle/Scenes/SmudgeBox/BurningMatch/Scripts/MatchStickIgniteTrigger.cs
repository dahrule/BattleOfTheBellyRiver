using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class MatchStickIgniteTrigger : MonoBehaviour
{
    [Tooltip("Object with this tag can trigger ignition.")]
    readonly string matchboxTag="matchbox";

    [Tooltip("The rigidbody of the matchstick.")]
    [SerializeField] Rigidbody rigidBody;

    [SerializeField] XRGrabInteractable grabInteractable;

    [Tooltip("The forward vector that indicates the direction of match movement for ignition to occur.")]
    [SerializeField] Transform matchIgniteForward = null;

    [Tooltip("How accurate the match movement direction should be in relation to the forward reference for ignition to occur: 1 for perfect aligment, 0 no aligment required.")]
    [Range(0, 1)] [SerializeField] float movementDirectionThreshold = 0.95f;

    [Tooltip("How fast the match movement should be for ignition to occur")]
    [Range(0, 1)] [SerializeField] float speedTreshold = 0.6f;

    [SerializeField] UnityEvent OnIgniteActivated;

    Vector3 previousPosition;
    float lastUpdateTime;

    Vector3 velocity;
    float speed;

    bool insideFrictionzone = false;

    void Start()
    {
        grabInteractable.movementType = XRBaseInteractable.MovementType.VelocityTracking;

        // Initialize the previous position and last update time
        previousPosition = transform.position;
        lastUpdateTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (insideFrictionzone==false) return;

        // Calculate the time elapsed since the last update
        float deltaTime = Time.time - lastUpdateTime;

        // Calculate match's displacement vector
        Vector3 displacement = transform.position - previousPosition;

        velocity = displacement / deltaTime;

        speed = velocity.magnitude;

        // Update the previous position and last update time
        previousPosition = transform.position;
        lastUpdateTime = Time.time;

        // Calculate the dot product between velocity and the forward reference vector
        float dotProduct = Vector3.Dot(velocity.normalized, matchIgniteForward.transform.forward);

        // Check match speed and movement accuracy thresholds to trigger ignition
        if (Mathf.Abs(dotProduct)>=movementDirectionThreshold && speed>= speedTreshold)
        {
            OnIgniteActivated?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.CompareTag(matchboxTag))
        {
            insideFrictionzone = true;
            ConstrainMovement(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(matchboxTag))
        {
            insideFrictionzone = false;
            ConstrainMovement(false);
        }  
    }

    // Guides the user's movement to stick to the surface of the matchbox by limiting the rotation of the matchstick. The XRGrabInteractable  MovementType property must be set as Velocity Tracking.
    private void ConstrainMovement(bool canRotate)
    {
        if (rigidBody)
        {
            rigidBody.freezeRotation = canRotate;
        }
    }
}
