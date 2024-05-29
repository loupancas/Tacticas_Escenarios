using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Dash
{
    CountdownTimer _timer, _delayedTimer, _chargedDashTimer, _dashToDashTimer;
    ModifierStat _buffs;
    FirstPersonPlayer _pj;
    Rigidbody _rb;
    float _dashDuration;
    float _dashSpeed;
    float _maxCountDashs;
    float _currCountDashs;
    Vector3 _delayedForceToApply;
    TextoActualizable _texto;
    public player_Dash(FirstPersonPlayer pj, ModifierStat buffs, TextoActualizable texto)
    {
        _dashDuration = buffs.StatResultado.dashTime;
        _dashSpeed = buffs.StatResultado.dashSpeed;
        _maxCountDashs = buffs.StatResultado.maxDashsCount;
        _dashToDashTimer = new CountdownTimer(0.5f);
        _timer = new CountdownTimer(_dashDuration);
        _delayedTimer = new CountdownTimer(0.025f);
        _chargedDashTimer = new CountdownTimer(buffs.StatOriginal.chargeDashTime);
        _chargedDashTimer.OnTimerStop = ChargeDash;
        _timer.OnTimerStop = ResetDash;
        _delayedTimer.OnTimerStop = DelayedDashForce;
        _dashToDashTimer.OnTimerStop = _dashToDashTimer.Reset;
        _pj = pj;
        _rb = pj.gameObject.GetComponent<Rigidbody>();
        _buffs = buffs;
        _texto = texto;
        _currCountDashs = _maxCountDashs;
        _texto.UpdateHUD(_currCountDashs, _maxCountDashs, " Dashs");
    }

    public void UpdateDashTimer(float TimeDelta)
    {
        _timer.Tick(TimeDelta);
        _delayedTimer.Tick(TimeDelta);
        _chargedDashTimer.Tick(TimeDelta);
        _dashToDashTimer.Tick(TimeDelta);
    }

    public void Dash(float xAxis, float zAxis)
    {
        if (_currCountDashs <= 0 || _dashToDashTimer.IsRunning)
            return;

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
        _chargedDashTimer.Start();
        _dashToDashTimer.Start();
        _pj.dashing = true;
        _rb.useGravity = false;
        _currCountDashs--;
        _texto.UpdateHUD(_currCountDashs, _maxCountDashs, " Dashs");
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

    void ChargeDash()
    {
        Debug.Log("Cargando dash");
        _currCountDashs++;
        _texto.UpdateHUD(_currCountDashs, _maxCountDashs, " Dashs");
        if (_currCountDashs < _maxCountDashs)
        {
            _chargedDashTimer.Reset();
            _chargedDashTimer.Start();
        } 
    }
}
