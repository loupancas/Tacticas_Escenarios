using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class TrackEnemigoVolador : MonoBaseState
{
    [SerializeField] EnemigoVolador _me;
    [SerializeField] Rigidbody _rb;
    Vector3 _velocity;
    [SerializeField] float _maxVelocity;
    [SerializeField] float _maxForce;

    public TrackEnemigoVolador(EnemigoVolador me, float maxVelocity, float maxForce)
    {
        _me = me;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    public override IState ProcessInput()
    {

        if (_me.IsAttackDistance() && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        if (!_me.InLineOfSight(transform.position, GameManager.instance.pj.transform.position) && Transitions.ContainsKey(StateTransitions.ToSearch))
            return Transitions[StateTransitions.ToSearch];

        return this;
    }

    public override void UpdateLoop()
    {
        AddForce(Seek(GameManager.instance.pj.transform.position));

        _me.transform.position += _velocity * Time.deltaTime;
        _me.transform.forward = _velocity;
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
