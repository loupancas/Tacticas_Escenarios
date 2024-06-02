using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMach;

public class EnemyIdleState : MonoBaseState
{
    [SerializeField] Enemyfsm owner;

    public override IState1 ProcessInput()
    {
        if(owner.IsAttackDistance() && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        if(owner.IsPersuitDistance() && Transitions.ContainsKey(StateTransitions.ToPersuit))
            return Transitions[StateTransitions.ToPersuit];

        return this;
    }

    public override void Enter(IState1 from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        Debug.Log("Estoy en idle");
    }

    public override void UpdateLoop()
    {

    }
}
