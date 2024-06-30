using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class SeparationEnemigoVolador : MonoBaseState
{
    [SerializeField] EnemigoVolador _me;
    Vector3 _velocity;
    [SerializeField] float _maxVelocity;
    [SerializeField] float _maxForce;

    public SeparationEnemigoVolador(EnemigoVolador me, float maxVelocity, float maxForce)
    {
        _me = me;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
    }

    public override IState ProcessInput()
    {
        if ((!_me.IsSeparationDistance() && Transitions.ContainsKey(StateTransitions.ToPersuit)) || (GameManager.instance.arenaManager.enemigosEnLaArena.Count == 0))
            return Transitions[StateTransitions.ToPersuit];

        return this;
    }

    public override void UpdateLoop()
    {
        AddForce(Separation(GameManager.instance.arenaManager.enemigosEnLaArena, 1f));

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

    Vector3 Separation(List<EnemigoBase> boids, float radius)
    {
        Vector3 desired = Vector3.zero;

        foreach (var item in boids)
        {
            var dir = item.transform.position - _me.transform.position;
            if (dir.magnitude > radius || item == this)
                continue;

            desired -= dir;
        }

        if (desired == Vector3.zero)
            return desired;

        desired.Normalize();
        desired *= _maxVelocity;

        return CalculateSteering(desired);
    }

    Vector3 CalculateSteering(Vector3 desired)
    {
        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
    }
}
