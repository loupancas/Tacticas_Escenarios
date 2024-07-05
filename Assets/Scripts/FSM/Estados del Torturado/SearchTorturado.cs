using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using UnityEngine.AI;

public class SearchTorturado : MonoBaseState
{
    NavMeshAgent _agent;
    Torturado _me;
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        _agent = gameObject.GetComponent<NavMeshAgent>();
        _me = gameObject.GetComponent<Torturado>();
    }

    public override IState ProcessInput()
    {
        if (_me.InLineOfSight(transform.position, GameManager.instance.pj.transform.position) && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        return this;
    }

    public override void UpdateLoop()
    {
        _agent.destination = GameManager.instance.pj.transform.position;
    }

    
}
