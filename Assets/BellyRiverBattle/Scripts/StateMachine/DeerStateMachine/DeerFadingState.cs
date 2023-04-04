using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerFadingState : DeerBaseState
{
    readonly SkinnedMeshDissolver _meshDissolver;
    readonly int _alertHash = Animator.StringToHash("alert");

    //Constructor
    public DeerFadingState(DeerStateMachine stateMachine, SkinnedMeshDissolver meshDissolver) : base(stateMachine)
    {
        _meshDissolver = meshDissolver;
    }

    public override void Enter()
    {
        _stateMachine.AnimController.SetTrigger(_alertHash);
        _meshDissolver.Dissolve();
    }

    public override void Exit()
    {
        
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
    }
}
