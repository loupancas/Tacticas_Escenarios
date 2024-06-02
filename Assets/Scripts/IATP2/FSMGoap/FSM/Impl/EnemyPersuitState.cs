using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMach;

public class EnemyPersuitState : MonoBaseState
{
    [SerializeField] Enemyfsm owner;

    [SerializeField] float speed = 5;

    public override IState1 ProcessInput()
    {
        if (owner.IsAttackDistance() && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        if (!owner.IsPersuitDistance() && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];

        return this;
    }

    public override void UpdateLoop()
    {
        var dir = (owner.target.position - owner.transform.position).normalized;

        owner.transform.position += dir * Time.deltaTime * speed;
    }
}
