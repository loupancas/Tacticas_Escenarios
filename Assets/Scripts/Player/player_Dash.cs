using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Dash
{
    CountdownTimer _timer, _delayedTimer;
    ModifierStat _buffs;


    FirstPersonPlayer _pj;
    Rigidbody _rb;

    float _dashForce;
    float _dashUpwardForce;
    float _dashDuration;
    float _dashFov;
    float _dashSpeed;
    Vector3 _delayedForceToApply;
    public player_Dash(float timeDash, FirstPersonPlayer pj, ModifierStat buffs)
    {
        _dashDuration = timeDash;
        _timer = new CountdownTimer(_dashDuration);
        _pj = pj;
        _rb = pj.GetComponent<Rigidbody>();
        _buffs = buffs;
    }

    public void UpdateDashTimer(float TimeDelta)
    {
        _timer.Tick(TimeDelta);
        _delayedTimer.Tick(TimeDelta);

        if (_delayedTimer.IsFinished)
        {
            _rb.velocity = Vector3.zero;
            _rb.AddForce(_delayedForceToApply, ForceMode.Impulse);
        }

        if (_timer.IsFinished)
        {
            _pj.dashing = false;
        }
    }

    public void Dash(float xAxis, float zAxis)
    {
        _timer.Start();
        Vector3 dir = (_pj.transform.right * xAxis + _pj.transform.forward * zAxis).normalized;
        _buffs.Add("Dash", (Stats original) => { original.movementSpeed = _dashSpeed; return original; }, _dashDuration);
        Vector3 forceToApply;

        if(xAxis != 0 || zAxis != 0)
        {
            forceToApply = _pj.transform.forward * _buffs.StatResultado.dashForce + _pj.transform.up * _buffs.StatResultado.dashUpwardForce;
        }
        else
        {
            forceToApply = dir * _buffs.StatResultado.dashForce + _pj.transform.up * _buffs.StatResultado.dashUpwardForce;
        }
        _delayedForceToApply = forceToApply;
        _delayedTimer = new CountdownTimer(0.025f);
        _delayedTimer.Start();
        _pj.dashing = true;
    }

}
