using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class TrackEnemigoVolador : MonoBaseState
{
    [SerializeField] EnemigoVolador me;
    Vector3 _velocity;
    [SerializeField] float _maxVelocity;
    [SerializeField] float _maxForce;
    [SerializeField] float _distanceToAttack;
    [SerializeField] float _distanceToSeparation;

    public override IState ProcessInput()
    {
        var Distance = (Vector3.Distance(transform.position, GameManager.instance.pj.transform.position));

        if (me.IsAttackDistance() && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        if (me.IsSeparationDistance() && Transitions.ContainsKey(StateTransitions.ToSeparation))
            return Transitions[StateTransitions.ToSeparation];

        return this;
    }

    public override void UpdateLoop()
    {
        AddForce(Seek(GameManager.instance.pj.transform.position));

        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
    }
    Vector3 Seek(Vector3 dir)
    {
        var desired = dir - transform.position;
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
