using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMach;

public class EnemyAttackState : MonoBaseState
{
    [SerializeField] float cd;

    float timer;

    public override IState1 ProcessInput()
    {
        if(timer >= cd && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];

        return this;
    }

    public override void Enter(IState1 from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        Debug.Log("Ataco");
    }

    public override Dictionary<string, object> Exit(IState1 to)
    {
        timer = 0;
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        timer += Time.deltaTime;

    }
}
