using UnityEngine;
using UnityEngine.AI;

public class Mover : NavmeshAgentMover
{
    readonly int _forwardSpeedHash = Animator.StringToHash("forwardSpeed");
    readonly int _moveHash = Animator.StringToHash("move");

    [Header("Character steering")]
    [Tooltip("Overwrites acceleration property of the navmesh agent")]
    [SerializeField] float _defaultAcceleration = 1f;
    public float DefaultAcceleration { get { return _defaultAcceleration; } }

    public bool IsMoving { get { return _agent.velocity.magnitude>=Mathf.Epsilon; }}

    private void Start()
    {
        SetAcceleration(_defaultAcceleration);
    }

    //To be used with Unity events
    public void MoveTo(Transform target)
    {
        MoveTo(target.position);
    }
    public new void MoveTo(Vector3 destination)
    {
        if (_animController != null)_animController.SetTrigger(_moveHash);
        _agent.destination = destination;
    }

   protected override void UpdateAnimator()
    {
        if (_animController == null) return;

        Vector3 velocity = _agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;

       _animController.SetFloat(_forwardSpeedHash, speed);
               
    }
    public void SetAcceleration(float acceleration)
    {
        _agent.acceleration =acceleration;
    }

    public override void StartMoveAction(Vector3 destination)
    {
        MoveTo(destination);
    }
}
