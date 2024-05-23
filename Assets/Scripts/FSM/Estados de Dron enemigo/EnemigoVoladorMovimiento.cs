using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoVoladorMovimiento : IState
{
    FSM _fsm;
    Transform _transform;
    float _maxVelocity;
    float _maxForce;
    float _viewRadius;
    float _viewAngle;
    Vector3 _velocity;
    LayerMask _wallLayer;

    public EnemigoVoladorMovimiento(FSM fsm, Transform transform, float maxVelocity, float maxForce, float viewRadius, float viewAngle, LayerMask wallLayer)
    {
        _fsm = fsm;
        _transform = transform;
        _maxVelocity = maxVelocity;
        _maxForce = maxForce;
        _viewRadius = viewRadius;
        _viewAngle = viewAngle;
        _wallLayer = wallLayer;
    }

    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        if(InLineOfSight(_transform.position, GameManager.instance.pj.transform.position))
        {
            AddForce(Seek(GameManager.instance.pj.transform.position));
        }
        
        _transform.position += _velocity * Time.deltaTime;
        _transform.forward = _velocity;

        if(Vector3.Distance(_transform.position, GameManager.instance.pj.transform.position ) < 5)
        {
            _fsm.ChangeState("Attack");
        }
    }

    void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, _maxVelocity);
    }
    Vector3 Seek(Vector3 dir)
    {
        var desired = dir - _transform.position;
        desired.Normalize();
        desired *= _maxVelocity;

        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
    }
    protected bool InLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, _wallLayer);

    }

    public bool InFOV(Transform obj)
    {
        var dir = obj.position - _transform.position;

        if (dir.magnitude < _viewRadius)
        {
            if (Vector3.Angle(_transform.forward, dir) <= _viewAngle * 0.5f)
            {
                return InLineOfSight(_transform.position, obj.position);
            }
        }

        return false;
    }
}

