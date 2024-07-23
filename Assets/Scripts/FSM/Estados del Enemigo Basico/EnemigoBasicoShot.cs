using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class EnemigoBasicoShot : MonoBaseState
{
    EnemigoBasico _me;
    [SerializeField] ArmaEnemigo _gun;
    [SerializeField] ProyectilesBase _proyectiles;
    [SerializeField] GameObject _spawnPoint;

    [SerializeField] float _cooldownShot;
    CountdownTimer _timer;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        _me = gameObject.GetComponent<EnemigoBasico>();

        _gun.gameObject.SetActive(true);

        _timer = new CountdownTimer(_cooldownShot);
        _timer.OnTimerStop = Shot;
        _timer.Start();
    }

    public override IState ProcessInput()
    {
        if ((!_me.IsAttackDistance() || !_me.InLineOfSight(transform.position, GameManager.instance.pj.transform.position)) && Transitions.ContainsKey(StateTransitions.ToPersuit))
            return Transitions[StateTransitions.ToPersuit];

        return this;
    }

    public override void UpdateLoop()
    {
        _timer.Tick(Time.deltaTime);

        Vector3 dir = new Vector3(GameManager.instance.pj.transform.position.x, 0, 0);
        
    }

    public void Shot()
    {
        _proyectiles.SpawnProyectile(_spawnPoint.transform);

        _timer.Reset();
        _timer.Start();
    }

}
