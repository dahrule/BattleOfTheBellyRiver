#define testineditor
using UnityEngine;
using System;

/// <summary>
/// Implements the grazing behavior. Currently it only runs a feeding on grass animation.
/// TODO: Move between local grass points every random seconds.
/// </summary>
public class DeerGrazingState : DeerBaseState
{
    WayPointPath _grazingPath;
    int _currentWaypointIndex;
    float _waypointTolerance = 0.5f;
    readonly int _grazingIdleHash = Animator.StringToHash("grazingIdle");
    readonly float _secondsWithinAlertState=30f;
    readonly float _walkAcceleration = 0.2f;

    //Constructor
    public DeerGrazingState(DeerStateMachine stateMachine, WayPointPath localPath) : base(stateMachine)
    {
        _grazingPath = localPath;
    }

    public override void Enter()
    {
        RandomizePath();

        //Change navemsh acceleratioi
       _stateMachine.Mover.SetAcceleration(_walkAcceleration);

        //Unparent path from deer
        _grazingPath.transform.SetParent(null);

        //Run grazing animation.
        _stateMachine.AnimController.SetTrigger(_grazingIdleHash);

        //TODO: Start random timer.

    }

    public override void Exit()
    {
        //parent path to deer again.
        _grazingPath.transform.SetParent(_stateMachine.transform);

        //TODO: Force stop timer.
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        //Go to Alert state when player is close.
        bool playerismoving=_stateMachine.Mover.IsMoving;
    #if testineditor
        playerismoving=true;
    #endif
        if (_stateMachine.DistanceToPlayer() < _stateMachine.AlertDistance && playerismoving)
            _stateMachine.Switch(new DeerAlertState(_stateMachine, _secondsWithinAlertState));

        //Trigger feed animation when reaching a new waypoint.
        if (AtWaypoint())
        {
            _stateMachine.AnimController.SetTrigger(_grazingIdleHash);
            _currentWaypointIndex = _grazingPath.GetNextIndex(_currentWaypointIndex);
        }

    }
    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(_stateMachine.transform.position, _grazingPath.GetWaypointPos(_currentWaypointIndex));
        return distanceToWaypoint < _waypointTolerance;
    }
    
    private void RandomizePath()
    {
        //Add variation to the behavior by randomly setting the starting waypoint of the path
        //and rotating the path.
        _currentWaypointIndex = UnityEngine.Random.Range(0, _grazingPath.NumberOfWaypoints);
        
        float randomAngle = UnityEngine.Random.Range(0f, 355f);
        _grazingPath.transform.Rotate(new Vector3(0f, randomAngle, 0f));
    }

    private void Move() //will be called when random timer ends.
    {
        Transform target = _grazingPath.GetWaypoint(_currentWaypointIndex);
        _stateMachine.Mover.MoveTo(target);
        
    }

}
