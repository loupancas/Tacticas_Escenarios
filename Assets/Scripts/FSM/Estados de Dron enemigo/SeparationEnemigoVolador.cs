using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class SeparationEnemigoVolador : MonoBaseState
{
    [SerializeField] float _distanceToSeparation;
    Vector3 _velocity;
    [SerializeField] float _maxVelocity;
    [SerializeField] float _maxForce;
    public override IState ProcessInput()
    {
        var Distance = (Vector3.Distance(transform.position, GameManager.instance.pj.transform.position));
        foreach (EnemigoBase a in GameManager.instance.arenaManager.enemigosEnLaArena)
        {
            if (a == this)
                continue;

            if (Distance > _distanceToSeparation)
            {
                return Transitions[StateTransitions.ToPersuit];
            }
        }

        return this;
    }

    public override void UpdateLoop()
    {
        AddForce(Separation(GameManager.instance.arenaManager.enemigosEnLaArena, 1f));

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

    Vector3 Separation(List<EnemigoBase> boids, float radius)
    {
        Vector3 desired = Vector3.zero;

        foreach (var item in boids)
        {
            var dir = item.transform.position - transform.position;
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
