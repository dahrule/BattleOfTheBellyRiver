using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Moves the deer sequencially between waypoints of the deer's path using the jumping animation.
/// *Abruptly increases the deer acceleration at start to trigger the jump animation instantly.
/// *Exits when the deer reaches the target waypoint or the end of the path.
/// </summary>
public class DeerFleeState : DeerBaseState
{
    Transform _target;
    readonly float _waypointTolerance = 0.5f;
    readonly float _fleeAcceleration = 3f;

    //Constructor
    public DeerFleeState(DeerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        //Change navmesh acceleration for a faster flee reaction.
        //*animation is changed in the mover script when MovedTo() is called.
        _stateMachine.Mover.SetAcceleration(_fleeAcceleration);
        
        //Move to waypoint target.
        _target = _stateMachine.DeerPath.GetWaypoint(_stateMachine.CurrentWaypointindex);
        _stateMachine.Mover.MoveTo(_target);
    }

    public override void Exit()
    {
        //Set next waypoint.
        _stateMachine.CurrentWaypointindex = _stateMachine.DeerPath.GetNextIndex(_stateMachine.CurrentWaypointindex);

        //Reset navmesh acceleration to the mover default.
        _stateMachine.Mover.SetAcceleration(_stateMachine.Mover.DefaultAcceleration);
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
       
        if (!AtWaypoint()) return;

        if (EndOfPath())
        {
            _stateMachine.Switch(new DeerFadingState(_stateMachine, _stateMachine.SkinMeshDissolver));
            return;
        }

        //Invoke OnPathWaypointReached event.
        _stateMachine.OnPathWaypointReached?.Invoke();
        //Look at player when reaching waypoint (Switch to alert state with a short time). 
        _stateMachine.Switch(new DeerAlertState(_stateMachine, 2f));
        
    }
    private bool EndOfPath()
    {
        return _stateMachine.CurrentWaypointindex == _stateMachine.DeerPath.NumberOfWaypoints-1;
    }
    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(_stateMachine.transform.position, _target.position);
        return distanceToWaypoint < _waypointTolerance;
    }
}
