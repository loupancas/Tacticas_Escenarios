using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class AttackEnemigoVolador : MonoBaseState
{
    [SerializeField] EnemigoVolador me;
    [SerializeField] float _distanceToAttack;
    [SerializeField] float _cdShot;
    [SerializeField] CountdownTimer _timer;
    [SerializeField] ProyectilesBase _proyectiles;
    [SerializeField] Transform _spawnPoint;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        _timer = new CountdownTimer(me.cdShot);
        _timer.OnTimerStop = Disparar;
    }
    public override IState ProcessInput()
    {
        if (!me.IsAttackDistance() && Transitions.ContainsKey(StateTransitions.ToPersuit))
            return Transitions[StateTransitions.ToPersuit];

        return this;
    }

    public override void UpdateLoop()
    {
        transform.LookAt(GameManager.instance.pj.transform);

        _timer.Tick(Time.deltaTime);
    }

    public void Disparar()
    {
        _proyectiles.SpawnProyectile(_spawnPoint);
        _timer.Reset();
    }
}
