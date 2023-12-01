using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class NavmeshAgentMover : MonoBehaviour
{
    protected NavMeshAgent _agent;
    protected Animator _animController;

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animController = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if(_animController) UpdateAnimator();
    }

    public void MoveTo(Vector3 destination)
    {
        _agent.destination = destination;
        _agent.isStopped = false;
    }

    protected abstract void UpdateAnimator();
   

    public void Cancel()
    {
        _agent.isStopped = true;
    }

    public abstract void StartMoveAction(Vector3 destination);

}

