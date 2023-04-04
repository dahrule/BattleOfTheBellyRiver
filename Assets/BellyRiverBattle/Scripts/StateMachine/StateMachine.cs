using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    State _currentState;

    private void Update()
    {
        _currentState?.Tick(Time.deltaTime);
    }

    public void Switch(State newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }
}
