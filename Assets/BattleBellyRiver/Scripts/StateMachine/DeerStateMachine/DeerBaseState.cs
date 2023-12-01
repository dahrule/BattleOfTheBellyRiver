
public abstract class DeerBaseState:State
{
    protected DeerStateMachine _stateMachine;

    //Constructor
    public DeerBaseState(DeerStateMachine stateMachine) 
    {
        _stateMachine = stateMachine;
    }

    public override void Tick(float deltaTime)
    {
        if (_stateMachine.Logger == null) return;
        _stateMachine.Logger.Log("In " + this.GetType().Name, _stateMachine);
    }
}
