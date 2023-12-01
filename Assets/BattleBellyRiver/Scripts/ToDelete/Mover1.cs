using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover1 : MonoBehaviour
{
    NavMeshAgent _agent;
    Animator _animController;

    readonly int _forwardSpeedHash = Animator.StringToHash("forwardSpeed");
    readonly int _moveHash = Animator.StringToHash("move");

    [Header("Character steering")]
    [Tooltip("Overwrites acceleration property of the navmesh agent")]
    [SerializeField] float _defaultAcceleration = 1f;

    [Tooltip("Minimum Speed at which the agent is considered to be moving")]
    [SerializeField] float _movementTestTreshold = 0.05f;
    public float DefaultAcceleration { get { return _defaultAcceleration; } }

    public bool IsMoving { get { return _agent.velocity.magnitude>=_movementTestTreshold; }}

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animController = GetComponent<Animator>(); 
    }
    private void Start()
    {
        SetAcceleration(_defaultAcceleration);
    }

    private void Update()
    {
        UpdateAnimator();
    }

    //To be used with Unity events
    public void MoveTo(Transform target)
    {
        if (_animController != null) _animController.SetTrigger(_moveHash);
        _agent.destination = target.position;
    }
    public void MoveTo(Vector3 destination)
    {
        if (_animController != null)_animController.SetTrigger(_moveHash);
        _agent.destination = destination;
    }

    private void UpdateAnimator()
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

    
}
