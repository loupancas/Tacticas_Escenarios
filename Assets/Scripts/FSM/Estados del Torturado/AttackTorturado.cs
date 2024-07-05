using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using UnityEngine.AI;
public class AttackTorturado : MonoBaseState
{
    NavMeshAgent _agent;
    Torturado _me;
    Transform _position;
    CountdownTimer _timer;
    bool _OnCharge;
    [SerializeField] float _speedCharge;
    [SerializeField] float _timeToCharge;

    private void Start()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _me = gameObject.GetComponent<Torturado>();
        _agent.speed = _speedCharge;
        
        _timer = new CountdownTimer(_timeToCharge);

        _timer.OnTimerStart = OnCharge;
        _timer.OnTimerStop = SetTarget;
        _timer.Start();
    }

    public override IState ProcessInput()
    {
        return this;
    }

    public override void UpdateLoop()
    {
        _timer.Tick(Time.deltaTime);
        print(_timer.Progress);
        if(_OnCharge)
        {
            _agent.destination = transform.forward;
        }
        
    }


    public void OnCharge()
    {
        transform.LookAt(GameManager.instance.pj.transform.position);
    }

    public void SetTarget()
    {
        _OnCharge = true;
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        return base.Exit(to);

        
    }

}
