using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Procedurally animates the deer's neck to look at or away from the player 
/// when entering or leaving the state, respectevily.
/// Alert state ends when the countup timer stops or when the player gets close enough to the deer
/// (i.e.,reaches the elusive distance threshold).
/// </summary>
public class DeerAlertState_unrefactor : DeerBaseState
{
    private const float CancelAimTreshold = 0.01f;
    Vector3 _aimOffset = new(0f, 1f, 0f); //offset the loo

    readonly int _alertHash = Animator.StringToHash("alert");
    float _headRotSpeed;
    readonly float _stateStopTime;
    float _targetWeight;

    readonly float _angleOfSight=24f;

    IEnumerator _countUp;
    event Action OnCancelAimComplete;
    event Action OnCountUpStop;

    bool hashappened = false;
    //Constructor
    public DeerAlertState_unrefactor(DeerStateMachine stateMachine, float stoptime) : base(stateMachine)
    {
        _headRotSpeed = _stateMachine.HeadRotationSpeed_stress;
        _stateStopTime = stoptime;
    }
    public override void Enter()
    {
        //Subscribe to events
        OnCountUpStop += CancelAim;
        OnCancelAimComplete += SwitchToGrazing; //default event handler; Changes over some conditions.

        //initialize valules;
        _targetWeight = 1f;

        //Change to base alert animation.
        _stateMachine.AnimController.SetTrigger(_alertHash);

        //Start alert timer.
        _countUp = CountUpRoutine(_stateStopTime);
        _stateMachine.StartCoroutine(_countUp);

    }

    public override void Exit()
    {
        _stateMachine.RigControl.weight = 0;
    }
    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        //Update aim target position.
        _stateMachine.LookAtTarget.transform.position = _stateMachine.Player.position + _aimOffset;

        //Activate Aim gradually
        _stateMachine.RigControl.weight = Mathf.Lerp(_stateMachine.RigControl.weight, _targetWeight, Time.deltaTime * _headRotSpeed);

        if (_stateMachine.RigControl.weight < CancelAimTreshold)
            OnCancelAimComplete?.Invoke();

       //Fix head flickering ocurring when aim target is behind deer
       FixHeadFlickering();

        //Flee away, if player gets too close.
        if (_stateMachine.DistanceToPlayer() < _stateMachine.ElusiveDistance)
        {
            if (!hashappened)
            {
                _stateMachine.StopCoroutine(_countUp);
                _headRotSpeed = _stateMachine.HeadRotationSpeed_stress * 2;
                CancelAim();
                OnCancelAimComplete -= SwitchToGrazing;
                OnCancelAimComplete += SwitchToFlee;

                hashappened = true;
            }

        }
    }

    private void CancelAim()
    {
        _targetWeight = 0f;
    }
    IEnumerator CountUpRoutine(float stopTime) //This is a timer that ends at stoptime.
    {
        float startTime = Time.time;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (Time.time - startTime > stopTime)
                break;
        }

        OnCountUpStop?.Invoke();
    }

    void SwitchToGrazing()
    {
        _stateMachine.Switch(new DeerGrazingState(_stateMachine, _stateMachine.GrazingPath));
    }
    void SwitchToFlee()
    {
        _stateMachine.Switch(new DeerFleeState(_stateMachine));
    }
    public void FixHeadFlickering()
    {   /// <summary>
        /// Fixes the head flickering bug (i.e., continuos left-right head movement) that occurs 
        /// when the aim target is close to 180 degrees of the deer (behind deer) during alert state. 
        /// </summary>

        //Get direction from target to deer, and flatten the vector
        Transform target = _stateMachine.Player;
        Transform deer = _stateMachine.gameObject.transform;

        Vector3 dirTotarget = target.position - deer.position;
        dirTotarget.y = 0; //flatten vector
        dirTotarget = Vector3.Normalize(dirTotarget);

        //Get deer's backward vector and flatten the vector
        Vector3 backward = -deer.forward;
        backward.y = 0; //flatten vector
        backward = Vector3.Normalize(backward);

        //Calculate angle between deer's backward vector and direction to target vector
        float angle = Vector3.SignedAngle(backward, dirTotarget, Vector3.up);

        //Displace aimOffset to right or left when target is within the angle of sight
        if (Mathf.Abs(angle) < _angleOfSight) 
        {
            _aimOffset.x = -2f * Mathf.Sign(angle);
        }
        else _aimOffset.x = 0;
      
        //Debugging: Show and print angles 
        /*Debug.Log("angle: " + angle);
        Debug.DrawLine(deer.position, deer.position + backward, Color.blue);
        Debug.DrawLine(deer.position, deer.position + dirTotarget, Color.magenta);*/
    }
}
