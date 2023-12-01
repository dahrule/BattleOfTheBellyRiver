using UnityEngine;

/*Character moves with root Animation and navmesh agent velocity matches animation velocity.*/
public class MoverBuffalo : NavmeshAgentMover,IAction
{
    Vector3 _worldDeltaPositioin;
    Vector2 _groundDeltaPositioin;
    Vector2 _velocity = Vector2.zero;

    void Start()
    {
        _agent.updatePosition = false;
    }

    protected override void UpdateAnimator()
    {
        CalculateVelocity();
        _animController.SetFloat("SpeedX", _velocity.x);//character forward direction.
        _animController.SetFloat("SpeedY", _velocity.y);
    }

    private void CalculateVelocity()
    {
        _worldDeltaPositioin = _agent.nextPosition - transform.position;
        _groundDeltaPositioin.x = Vector3.Dot(transform.right, _worldDeltaPositioin);
        _groundDeltaPositioin.y = Vector3.Dot(transform.forward, _worldDeltaPositioin);
        _velocity = (Time.deltaTime > 1e-5f) ? _groundDeltaPositioin / Time.deltaTime : _velocity = Vector2.zero;
    }

    private void OnAnimatorMove()
    {
        transform.position = _agent.nextPosition;
    }
    public override void StartMoveAction(Vector3 destination)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        MoveTo(destination);
    }

    public void MoveTo(Transform target)
    {
        StartMoveAction(target.position);
    }
}
