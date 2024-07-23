using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using UnityEngine.AI;

public class EnemigoBasicoMove : MonoBaseState
{
    NavMeshAgent _agent;
    EnemigoBasico _me;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        _me = gameObject.GetComponent<EnemigoBasico>();
        _agent = gameObject.GetComponent<NavMeshAgent>();
        

    }

    public override IState ProcessInput()
    {
        if ( _me.IsAttackDistanceMelee() && Transitions.ContainsKey(StateTransitions.ToMelee))
            return Transitions[StateTransitions.ToMelee];

        if (_me.IsHPLowThat(20) && _me.IsAttackDistance() && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        return this;
    }

    public override void UpdateLoop()
    {
        _agent.destination = GameManager.instance.pj.transform.position;

        print("Funcionando");
    }

}
