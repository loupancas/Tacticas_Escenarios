using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
public class ChargeTorturado : MonoBaseState
{
    CountdownTimer _timerToCharge;
    [SerializeField] float _timeToCharge;
    [SerializeField] float _distance;
    [SerializeField] Torturado _me;
    [SerializeField] GameObject _target;
    [SerializeField] bool _IsCharging;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        _timerToCharge = new CountdownTimer(_timeToCharge);

        _timerToCharge.OnTimerStart = OnCharge;
        _timerToCharge.OnTimerStop = Charge;

        _timerToCharge.Start();
        

    }
    public override IState ProcessInput()
    {
        if (!_me.InLineOfSight(transform.position, GameManager.instance.pj.transform.position) && Transitions.ContainsKey(StateTransitions.ToSearch))
            return Transitions[StateTransitions.ToSearch];

        if (_IsCharging && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        return this;
    }

    public override void UpdateLoop()
    {
        _timerToCharge.Tick(Time.deltaTime);
    }

    public void OnCharge()
    {
        transform.LookAt(GameManager.instance.pj.transform);
        _target.transform.position = new Vector3(GameManager.instance.pj.transform.position.x * _distance, 1, GameManager.instance.pj.transform.position.z * _distance);
    }

    public void Charge()
    {
        _IsCharging = true;
    }
}
