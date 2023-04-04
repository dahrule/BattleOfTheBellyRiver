using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A state for testing purposes.
/// </summary>
public class DeerTestState : DeerBaseState
{
    
    readonly int _alertHash = Animator.StringToHash("alert");


    Vector3 _DeerForward;
    Vector3 headForward;
    
    //Constructor
    public DeerTestState(DeerStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        _stateMachine.AnimController.SetTrigger(_alertHash);
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);


        _DeerForward = _stateMachine.transform.forward;
        headForward = _stateMachine.DeerHead.forward;
        //lookingForwad = Vector2.Dot(_HeadForwardAtStart, headForward) < 0.1;
        float dot = Vector3.Dot(_DeerForward, headForward);
        Debug.Log("DOT: "+dot);
        Debug.Log(dot>0.9);

       
        Debug.DrawRay(_stateMachine.DeerHead.position, _DeerForward * 10, Color.red);
        Debug.DrawRay(_stateMachine.DeerHead.position, headForward * 10, Color.blue);

    }

}
