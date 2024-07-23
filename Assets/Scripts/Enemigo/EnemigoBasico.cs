using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;

public class EnemigoBasico : EnemigoBase
{
    public EnemigoBasicoMove _moveState;
    public EnemigoBasicoAtaqueMelee _attackMeleeState;
    public EnemigoBasicoShot _shotState;

    [Header("Componentes")]
    [SerializeField] GameObject _gun;

    [Header("Stats de enemigo basico")]

    [SerializeField] float _distanceOfAttackMelee;
    [SerializeField] float _distanceOfDistanceAttack;

    private void Start()
    {
        _vida = _vidaMax;

        _fsm = new FiniteStateMachine(_moveState, StartCoroutine);

        _fsm.AddTransition(StateTransitions.ToPersuit, _attackMeleeState, _moveState);
        _fsm.AddTransition(StateTransitions.ToPersuit, _shotState, _moveState);

        _fsm.AddTransition(StateTransitions.ToMelee, _moveState, _attackMeleeState);

        _fsm.AddTransition(StateTransitions.ToAttack, _attackMeleeState, _shotState);
        _fsm.AddTransition(StateTransitions.ToAttack, _moveState, _shotState);

        OnlyPlan();
        _fsm.Active = true;
    }

    public override void Morir()
    {
        throw new System.NotImplementedException();
    }

    public override void SpawnEnemy(Transform spawnPoint)
    {
        throw new System.NotImplementedException();
    }

    public bool IsAttackDistance()
    {
        return Vector3.Distance(GameManager.instance.pj.transform.position, transform.position) <= _distanceOfDistanceAttack;
    }

    public bool IsAttackDistanceMelee()
    {
        return Vector3.Distance(GameManager.instance.pj.transform.position, transform.position) <= _distanceOfAttackMelee;
    }
    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, _wallLayer);
    }
    public bool IsHPLowThat(int vida)
    {
        return _vida <= vida;
    }

    private void OnlyPlan()
    {
        Func<float, bool> isDistance = a => a >= Vector3.Distance(GameManager.instance.pj.transform.position, transform.position);
        Func<Vector3, Vector3, bool> lineOfSight = (start, end) =>
        {
            var dir = end - start;

            return !Physics.Raycast(start, dir, dir.magnitude, _wallLayer);
        };

        Func<int, bool> IsHPLowThat = a => a <= _vida;

        Func<GameObject, bool> IsGunActive = a => a.activeInHierarchy;
        
        var actions = new List<GOAPAction>
        {
            new GOAPAction("Move")
                .Pre("IsPlayerInDistanceToMelee", isDistance(_distanceOfAttackMelee) == false)
                .Pre("IsPlayerInDistanceToShot", isDistance(_distanceOfDistanceAttack) == false)
                .Pre("IsHPLow", IsHPLowThat(20) == false)
                .Effect("IsPlayerInDistanceToMelee", isDistance(_distanceOfAttackMelee) == true)
                .LinkedState(_moveState),

            new GOAPAction("Melee")
                .Pre("IsPlayerInDistanceToMelee", isDistance(_distanceOfAttackMelee) == true)
                .Pre("IsHPLow", IsHPLowThat(20) == false)
                .LinkedState(_attackMeleeState),

            new GOAPAction("Shot")
                .Pre("IsPlayerInDistanceToShot", isDistance(_distanceOfDistanceAttack) == true)
                .Pre("IsHPLow", IsHPLowThat(20) == true)
                .Effect("IsGunActive", IsGunActive(_gun) == true)
                .LinkedState(_shotState),

            

        };
        //{
        //    new GOAPAction("Search")
        //        .Pre("isPlayerInSight",lineOfSight(transform.position, GameManager.instance.pj.transform.position) == false)
        //        .Effect("isPlayerInSight", lineOfSight(transform.position, GameManager.instance.pj.transform.position) == true)
        //        .LinkedState(searchState),

        //    new GOAPAction("Attack")
        //        .Pre("isPlayerInDistanceAttack", isDistance(_distanceToAttack) == true)
        //        .Effect("isPlayerAlive", false)
        //        .LinkedState(attackState),

        //    new GOAPAction("Chase")
        //        .Pre("isPlayerInSight", lineOfSight(transform.position, GameManager.instance.pj.transform.position) == true)
        //        .Effect("isPlayerInDistanceAttack", isDistance(_distanceToAttack) == true)
        //        .LinkedState(trackState),

        //    new GOAPAction("Lost View")
        //        .Pre("isPlayerOutOfArena", true)
        //        .LinkedState(lostViewState),
        //};


        var from = new GOAPState();
        //from.values["isPlayerInSight"] = false;
        //from.values["isPlayerNear"] = false;
        //from.values["isPlayerAlive"] = true;

        var to = new GOAPState();

        //to.values["isPlayerAlive"] = false;

        var planner = new GoapPlanner();
        planner.OnPlanCompleted += OnPlanCompleted;
        planner.OnCantPlan += OnCantPlan;

        planner.Run(from, to, actions, StartCoroutine);
    }

    private void OnPlanCompleted(IEnumerable<GOAPAction> plan)
    {
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

    private void OnCantPlan()
    {
        //TODO: debuggeamos para ver por qué no pudo planear y encontrar como hacer para que no pase nunca mas

        print("No se pudo planear nada");
    }
}
