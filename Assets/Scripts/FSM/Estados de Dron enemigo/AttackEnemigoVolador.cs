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
    [SerializeField] Rigidbody _rb;
    [SerializeField] LayerMask a;
    public AttackEnemigoVolador(EnemigoVolador me, ProyectilesBase proyectil, Transform spawnPoint)
    {
        _me = me;
        _proyectiles = proyectil;
        _spawnPoint = spawnPoint;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        _rb = gameObject.GetComponent<Rigidbody>();

        _rb.excludeLayers = a;

        _timer = new CountdownTimer(_me.cdShot);
        _timer.OnTimerStop = Disparar;
        _timer.OnTimerStart = Charge;
        _timer.Start();

        
        
    }
    public override IState ProcessInput()
    {
        if (!_me.IsAttackDistance() && Transitions.ContainsKey(StateTransitions.ToPersuit))
            return Transitions[StateTransitions.ToPersuit];

        if (!_me.InLineOfSight(transform.position, GameManager.instance.pj.transform.position) && Transitions.ContainsKey(StateTransitions.ToSearch))
            return Transitions[StateTransitions.ToSearch];

        return this;
    }

    public override void UpdateLoop()
    {
        _me.transform.LookAt(GameManager.instance.pj._head.transform);

        _timer.Tick(Time.deltaTime);
    }

    public void Charge()
    {
        //Aca se prepara para atacar
        _rb.velocity = Vector3.zero;
    }

    public void Disparar()
    {
        _proyectiles.SpawnProyectile(_spawnPoint);
        _timer.Reset();
        _timer.Start();
    }
}
