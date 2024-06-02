using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMach;
using Unity.VisualScripting;

public class Enemyfsm : MonoBehaviour
{
    public Transform target;

    [SerializeField] float distanceToAttack = 3;
    [SerializeField] float distanceToPersuit = 15;

    FiniteStateMachine1 fsm;

    [SerializeField] EnemyIdleState idleState;
    [SerializeField] EnemyPersuitState persuitState;
    [SerializeField] EnemyAttackState attackState;

    private void Start()
    {
        fsm = new FiniteStateMachine1(idleState, StartCoroutine);

        fsm.AddTransition(StateTransitions.ToPersuit, idleState, persuitState);
        fsm.AddTransition(StateTransitions.ToAttack, idleState, attackState);
        fsm.AddTransition(StateTransitions.ToAttack, persuitState, attackState);
        fsm.AddTransition(StateTransitions.ToIdle, persuitState, idleState);
        fsm.AddTransition(StateTransitions.ToIdle, attackState, idleState);


        fsm.Active = true;
    }

    public bool IsPersuitDistance()
    {
        return Vector3.Distance(target.position, transform.position) <= distanceToPersuit;
    }

    public bool IsAttackDistance()
    {
        return Vector3.Distance(target.position, transform.position) <= distanceToAttack;
    }
}
