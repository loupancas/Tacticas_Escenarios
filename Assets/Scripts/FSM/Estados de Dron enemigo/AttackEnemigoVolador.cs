using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class AttackEnemigoVolador : MonoBaseState
{
    [SerializeField] EnemigoVolador _me;
    [SerializeField] CountdownTimer _timer;
    [SerializeField] ProyectilesBase _proyectiles;
    [SerializeField] Transform _spawnPoint;

    public AttackEnemigoVolador(EnemigoVolador me, ProyectilesBase proyectil, Transform spawnPoint)
    {
        _me = me;
        _proyectiles = proyectil;
        _spawnPoint = spawnPoint;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        _timer = new CountdownTimer(_me.cdShot);
        _timer.OnTimerStop = Disparar;
        _timer.Start();
    }
    public override IState ProcessInput()
    {
        if (!_me.IsAttackDistance() && Transitions.ContainsKey(StateTransitions.ToPersuit))
            return Transitions[StateTransitions.ToPersuit];

        return this;
    }

    public override void UpdateLoop()
    {
        _me.transform.LookAt(GameManager.instance.pj._head.transform);

        _timer.Tick(Time.deltaTime);
    }

    public void Disparar()
    {
        _proyectiles.SpawnProyectile(_spawnPoint);
        _timer.Reset();
        _timer.Start();
    }
}
