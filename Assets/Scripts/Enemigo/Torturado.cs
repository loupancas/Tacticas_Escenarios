using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
public class Torturado : EnemigoBase
{
    [SerializeField] SearchTorturado searchState;
    [SerializeField] AttackTorturado attackState;
    [SerializeField] ChargeTorturado chargeState;

    [SerializeField] float _distanceToAttack;
    public override void Morir()
    {
        throw new System.NotImplementedException();
    }

    public override void SpawnEnemy(Transform spawnPoint)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        _vida = _vidaMax;

        _fsm = new FiniteStateMachine(searchState, StartCoroutine);

        _fsm.Active = true;


        _fsm.AddTransition(StateTransitions.ToCharge, searchState, chargeState);
        _fsm.AddTransition(StateTransitions.ToCharge, attackState, chargeState);
        
        _fsm.AddTransition(StateTransitions.ToSearch, chargeState, searchState);
        _fsm.AddTransition(StateTransitions.ToSearch, attackState, searchState);
        

        _fsm.AddTransition(StateTransitions.ToAttack, chargeState, attackState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, _wallLayer);
    }

    public bool IsAttackDistance()
    {
        return Vector3.Distance(GameManager.instance.pj.transform.position, transform.position) <= _distanceToAttack;
    }
}
