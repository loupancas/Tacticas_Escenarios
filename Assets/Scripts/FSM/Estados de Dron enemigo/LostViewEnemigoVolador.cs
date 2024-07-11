using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class LostViewEnemigoVolador : MonoBaseState
{
    [SerializeField] EnemigoVolador _me;
    [SerializeField] Vector3 _velocity;
    [SerializeField] float _maxVelocity;
    [SerializeField] float _maxForce;
    [SerializeField] CountdownTimer _timerToStand;
    [SerializeField] float _timeToStand;
    int _numeroRandom;
    Rigidbody _rb;
    public override IState ProcessInput()
    {
        if (_me.InLineOfSight(transform.position, GameManager.instance.pj.transform.position) && Transitions.ContainsKey(StateTransitions.ToPersuit))
            return Transitions[StateTransitions.ToPersuit];

        if (_me.InLineOfSight(transform.position, GameManager.instance.pj.transform.position) && _me.IsAttackDistance() && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        _timerToStand = new CountdownTimer(_timeToStand);

        _timerToStand.OnTimerStop = IrANodoRandom;

        _rb = gameObject.GetComponent<Rigidbody>();

    }

    public override void UpdateLoop()
    {

        _timerToStand.Tick(Time.deltaTime);

        Debug.Log("Tiempo de Time to stand: " + _timerToStand.Progress);

        if (Vector3.Distance(GameManager.instance.arenaManager.nodos[_numeroRandom].transform.position, transform.position) >= 0.5f && _timerToStand.IsFinished)
        {
            AddForce(Seek(GameManager.instance.arenaManager.nodos[_numeroRandom].transform.position));

            transform.position += _velocity * Time.deltaTime;
            transform.forward = _velocity;
        }
        else
        {
            _rb.velocity = Vector3.zero;
            
        }
        
    }

    public void IrANodoRandom()
    {
        _numeroRandom = Random.Range(0, GameManager.instance.arenaManager.nodos.Capacity);

        if(!GameManager.instance.arenaManager.InLineOfSight(transform.position, GameManager.instance.arenaManager.nodos[_numeroRandom].transform.position))
        {
            _numeroRandom = Random.Range(0, GameManager.instance.arenaManager.nodos.Capacity);
        }
        _timerToStand.Reset();
        _timerToStand.Start();
    }
    Vector3 Seek(Vector3 dir)
    {
        var desired = dir - _me.transform.position;
        desired.Normalize();
        desired *= _maxVelocity;

        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
    }
    void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, _maxVelocity);
    }
}
