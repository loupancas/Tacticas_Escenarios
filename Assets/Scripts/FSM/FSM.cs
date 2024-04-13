using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM 
{
    Dictionary<string, IState> _states = new Dictionary<string, IState>();

    IState _actualState;

    public void CreateState(string name, IState state)
    {
        if (!_states.ContainsKey(name))
            _states.Add(name, state);
    }

    public void Execute()
    {
        _actualState.OnUpdate();
    }

    public void ChangeState(string name)
    {
        if (_states.ContainsKey(name))
        {
            if (_actualState != null)
                _actualState.OnExit();

            _actualState = _states[name];
            _actualState.OnEnter();
        }
    }
}
