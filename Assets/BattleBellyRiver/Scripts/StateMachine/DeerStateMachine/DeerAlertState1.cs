using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Procedurally animates the deer's neck to look at or away from the player when entering or leaving the state respectively.
/// Alert state ends when the count-up timer stops or when the player trespasses the elusive distance threshold.
/// </summary>
public class DeerAlertState1 : DeerBaseState
{
    Vector3 _targetOffset = new(0f, 1f, 0f); //target offsets up from the target's position.
    readonly int _alertHash = Animator.StringToHash("alert");//base animation during the state.
    readonly float _angleOfSight=24f;//area behind the deer (24° left and right) where the target moves left or right from the player's center to fix the flickering bug.

    IEnumerator _countUp;
    IEnumerator _enableAim;
  
    event Action OnCountUpStop;

    readonly float _exitTime; //time to exit the state
    readonly float _headRotSpeed; //default deer's head rotation speed.
    Vector3 _targetPosition; //aim target
    Vector3 _playerPosition;
    readonly float _elusiveDistance;

    //Constructor
    public DeerAlertState1(DeerStateMachine stateMachine, float exitTime) : base(stateMachine)
    {
        _headRotSpeed = _stateMachine.HeadRotationSpeed_stress;
      
        _elusiveDistance = _stateMachine.ElusiveDistance;

        _exitTime = exitTime;
    }
    public override void Enter()
    {
        //Change to base alert animation. This animation is overwritten by the Rig controlling the neck rotation.
        _stateMachine.AnimController.SetTrigger(_alertHash);

        //Deer gradually turns head towards player. When AimStartRoutine ends, the alert timer starts.
        _enableAim = AimStartRoutine(_headRotSpeed,true,StartTimer);
        _stateMachine.StartCoroutine(_enableAim);
    }
    public override void Exit()
    {
        _stateMachine.RigControl.weight = 0;//Ensures the Rig component controlling the neck rotation deactivates.
    }
    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        //Update aim target position.
       _stateMachine.LookAtTarget.transform.position = _stateMachine.Player.position + _targetOffset;
        

        //Fix head flickering ocurring when aim target is behind deer.
        FixHeadFlickering();

        //Flee away, if player gets too close.
        if (_stateMachine.DistanceToPlayer() < _elusiveDistance)
        {
            //Force stop timer
            if(_countUp!=null)_stateMachine.StopCoroutine(_countUp); 

            if(_enableAim==null) CancelAim(_headRotSpeed*6, SwitchToFlee);
        }
        //Debug.Log(_aim);
    }
    IEnumerator AimStartRoutine(float headRotationSpeed,bool active, Action onComplete)
    {   // <summary>
        /// Gradually aim deer's head to look at (active=true) or away from the player (active=false).
        /// </summary>

    const float CancelAimTreshold = 0.1f; //helps completing the routine faster by slighlty reducing the distance between rigcontrol weight and target weight values. Should be close to zero.
    float targetWeight = active ? 1 : CancelAimTreshold;
        while(true)
        {
            _stateMachine.RigControl.weight = Mathf.Lerp(_stateMachine.RigControl.weight, targetWeight, Time.deltaTime * headRotationSpeed);
            _stateMachine.RigControl.weight = Mathf.Clamp01(_stateMachine.RigControl.weight);

            if (Mathf.Approximately(_stateMachine.RigControl.weight, targetWeight)) break;

            yield return null;
        }
        _enableAim = null;
        onComplete();
    }
    IEnumerator CountUpRoutine(float stopTime)
    {//<summary>
     //This is a timer that ends at stoptime.
     /// </summary>
     
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
            _targetOffset.x = -2f * Mathf.Sign(angle);
        }
        else _targetOffset.x = 0;
      
        //Debugging: Show and print angles 
        /*Debug.Log("angle: " + angle);
        Debug.DrawLine(deer.position, deer.position + backward, Color.blue);
        Debug.DrawLine(deer.position, deer.position + dirTotarget, Color.magenta);*/
    }
    void StartTimer()
    {
        OnCountUpStop +=()=>CancelAim(_headRotSpeed,SwitchToGrazing); //subscribe response for when timer ends.
        _countUp = CountUpRoutine(_exitTime);
        _stateMachine.StartCoroutine(_countUp);
    }
    void CancelAim(float timetoCancel,Action onComplete)
    {
        //Deer gradually turns head away from player. Once its done, executes whatever the onComplete funtion is.
        _enableAim = AimStartRoutine(timetoCancel,false,onComplete);
        _stateMachine.StartCoroutine(_enableAim);
    }
}
