using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class player_Movement
{
    FirstPersonPlayer _pj;
    ModifierStat _baseStatsPlayer;
    float _currDashTime;
    int _jumpsRemaining;
    float _mouseX;
    FirstPersonCamera _cam;
    Rigidbody _rb;
    TextoActualizable _textJumpCount;
    Controles _controles;
    public player_Movement(FirstPersonPlayer pj, FirstPersonCamera cam, Rigidbody rb, TextoActualizable text, ModifierStat baseStats, Controles controles)
    {
        _pj = pj;
        _cam = cam;
        _rb = rb;
        _baseStatsPlayer = baseStats;
        _textJumpCount = text;
        _controles = controles;
    }

    public void Movement(float xAxis, float zAxis)
    {
        Vector3 dir = (_pj.transform.right * xAxis + _pj.transform.forward * zAxis).normalized;

        _rb.MovePosition(_pj.transform.position += dir * _baseStatsPlayer.StatResultado.movementSpeed * Time.fixedDeltaTime);
    }

    public void Rotation(float xAxis, float yAxis)
    {
        _mouseX += xAxis * _controles.mouseSensitivity * Time.deltaTime;

        if (_mouseX >= 360 || _mouseX <= -360)
        {
            _mouseX -= 360 * Mathf.Sign(_mouseX);
        }

        yAxis *= _controles.mouseSensitivity * Time.deltaTime;

        _pj.transform.rotation = Quaternion.Euler(0, _mouseX, 0);
        _cam?.Rotate(_mouseX, yAxis);
    }

    public void Dash(float xAxis, float zAxis)
    {
        _currDashTime = 0;

        Vector3 dir = (_pj.transform.right * xAxis + _pj.transform.forward * zAxis).normalized;

        Vector3 forceToApply = Vector3.zero;

        if(xAxis != 0 || zAxis != 0)
        {
            forceToApply = dir * _baseStatsPlayer.StatResultado.dashForce + _pj.transform.up * _baseStatsPlayer.StatResultado.dashUpwardForce;
        }
        else
        {
            forceToApply = _pj.transform.forward * _baseStatsPlayer.StatResultado.dashForce + _pj.transform.up * _baseStatsPlayer.StatResultado.dashUpwardForce;
        }

        Debug.Log("Dash");
        Debug.Log("fuerzaAplicada: " + forceToApply);

        _pj.dashing = true;
        _rb.AddForce(forceToApply, ForceMode.Impulse);

        
    }



    public void Jump()
    {
        
        if (0 < (_jumpsRemaining - 1))
        {
            _rb.AddForce(_pj.transform.up * _baseStatsPlayer.StatResultado.jumpHeight, ForceMode.Impulse);
            
            _jumpsRemaining -= 1;
            

        }
        

        
    }
    
    public void DashState()
    {
        _currDashTime += Time.deltaTime;
        if (_currDashTime > _baseStatsPlayer.StatResultado.dashTime && _pj.dashing && _pj.grounded)
        {
            _pj.dashing = false;
            _rb.velocity = Vector3.zero;
        }
    }

    public void GroundedState()
    {
        float groundCheckDistance = 1.1f;
        RaycastHit hit;
        if (Physics.Raycast(_pj.transform.position, -_pj.transform.up, out hit, groundCheckDistance))
        {
            _pj.grounded = true;
            _jumpsRemaining = _baseStatsPlayer.StatResultado.maxJumpsCount;

            _textJumpCount.UpdateHUD(_jumpsRemaining, _baseStatsPlayer.StatResultado.maxJumpsCount, "Saltos");

        }
        else
        {
            _pj.grounded = false;
            _textJumpCount.UpdateHUD(_jumpsRemaining, _baseStatsPlayer.StatResultado.maxJumpsCount, "Saltos");
        }
            

        
        Debug.Log("Remaining jumps: " + _jumpsRemaining);
    }
}
