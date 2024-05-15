using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Dash
{
    CountdownTimer _timer, _delayedTimer;
    ModifierStat _buffs;
    FirstPersonPlayer _pj;
    Rigidbody _rb;
    float _dashDuration;
    float _dashSpeed;
    Vector3 _delayedForceToApply;
    public player_Dash(FirstPersonPlayer pj, ModifierStat buffs)
    {
        _dashDuration = buffs.StatResultado.dashTime;
        _dashSpeed = buffs.StatResultado.dashSpeed;
        _timer = new CountdownTimer(_dashDuration);
        _delayedTimer = new CountdownTimer(0.025f);
        _timer.OnTimerStop = ResetDash;
        _delayedTimer.OnTimerStop = DelayedDashForce;
        _pj = pj;
        _rb = pj.gameObject.GetComponent<Rigidbody>();
        _buffs = buffs;
    }

    public void UpdateDashTimer(float TimeDelta)
    {
        _timer.Tick(TimeDelta);
        _delayedTimer.Tick(TimeDelta);
    }

    public void Dash(float xAxis, float zAxis)
    {
        Debug.Log("Dash");
        Vector3 dir = (_pj.transform.right * xAxis + _pj.transform.forward * zAxis).normalized;
        _buffs.Add("Dash", (Stats original) => { original.movementSpeed = _dashSpeed; return original; }, _dashDuration);
        Vector3 forceToApply;

        _pj.cam.SetFov(50f);

        if(xAxis != 0 || zAxis != 0)
        {
            forceToApply = dir * _buffs.StatResultado.dashForce + _pj.transform.up * _buffs.StatResultado.dashUpwardForce;
        }
        else
        {
            forceToApply = _pj.transform.forward * _buffs.StatResultado.dashForce + _pj.transform.up * _buffs.StatResultado.dashUpwardForce;
        }
        _delayedForceToApply = forceToApply;
        _timer.Start();
        _delayedTimer.Start();
        _pj.dashing = true;
        _rb.useGravity = false;
    }

    void DelayedDashForce()
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(_delayedForceToApply, ForceMode.Impulse);
    }

    void ResetDash()
    {
        _pj.dashing = false;
        _rb.velocity = Vector3.zero;
        _pj.cam.SetFov(60f);
        _rb.useGravity = true;
    }
}
