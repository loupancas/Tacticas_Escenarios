using System.Collections.Generic;
using FSMach;
using UnityEngine;

public class GoapMiniTest : MonoBehaviour {

    public PatrolState patrolState;
    public ChaseState chaseState;
    public MeleeAttackState meleeAttackState;

    private FiniteStateMachine1 _fsm;


    void Start() {
        //OnlyPlan();
        PlanAndExecute();
    }

    private void OnlyPlan() {
    }

    private void PlanAndExecute() {
        var actions = new List<GOAPAction>{
                                              new GOAPAction("Patrol")
                                                 .Effect("isPlayerInSight", true)
                                                 .LinkedState(patrolState),

                                              new GOAPAction("Chase")
                                                 .Pre("isPlayerInSight", true)
                                                 .Effect("isPlayerNear",    true)
                                                 .LinkedState(chaseState),

                                              new GOAPAction("Melee Attack")
                                                 .Pre("isPlayerNear",   true)
                                                 .Effect("isPlayerAlive", false)
                                                 .LinkedState(meleeAttackState)
                                          };
        
        var from = new GOAPState();
        from.values["isPlayerInSight"] = false;
        from.values["isPlayerNear"]    = false;
        from.values["isPlayerAlive"]   = true;

        var to = new GOAPState();
        to.values["isPlayerAlive"] = false;

        var planner = new GoapPlanner();
        planner.OnPlanCompleted += OnPlanCompleted;
        planner.OnCantPlan      += OnCantPlan;

        planner.Run(from, to, actions, StartCoroutine);
    }


    private void OnPlanCompleted(IEnumerable<GOAPAction> plan) {
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }

    private void OnCantPlan() {
        //TODO: debuggeamos para ver por qué no pudo planear y encontrar como hacer para que no pase nunca mas
    }

}
