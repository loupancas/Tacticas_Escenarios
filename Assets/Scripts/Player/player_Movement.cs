using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class player_Movement
{
    FirstPersonPlayer _pj;
    ModifierStat _baseStatsPlayer;
    float _mouseX;
    FirstPersonCamera _cam;
    Rigidbody _rb;
    TextoActualizable _textJumpCount;
    Controles _controles;
    
    public player_Movement(FirstPersonPlayer pj, TextoActualizable text, ModifierStat baseStats, Controles controles)
    {
        _pj = pj;
        _cam = pj.cam;
        _rb = pj.GetComponent<Rigidbody>();
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

    
}
