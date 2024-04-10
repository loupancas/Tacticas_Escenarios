using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Movement
{
    FirstPersonPlayer _pj;
    float _movementSpeed;
    float _dashForce;
    float _dashUpwardForce;
    float _dashTime;
    float _jumpHeight;
    int _maxJumpsCount;
    int _jumpsRemaining;
    float _mouseSensitivity;
    float _mouseX;
    FirstPersonCamera _cam;
    Rigidbody _rb;
    

    public player_Movement(FirstPersonPlayer pj, float movementSpeed, float mouseSensitivity, float mouseX, FirstPersonCamera cam, Rigidbody rb, float dashForce, float dashUpwardForce, float jumpHeight, int maxJumpsCount)
    {
        _pj = pj;
        _movementSpeed = movementSpeed;
        _mouseSensitivity = mouseSensitivity;
        _mouseX = mouseX;
        _cam = cam;
        _rb = rb;
        _dashForce = dashForce;
        _dashUpwardForce = dashUpwardForce;
        _maxJumpsCount = maxJumpsCount;
        _jumpHeight = jumpHeight;
        
    }

    public void Movement(float xAxis, float zAxis)
    {
        Vector3 dir = (_pj.transform.right * xAxis + _pj.transform.forward * zAxis).normalized;

        _rb.MovePosition(_pj.transform.position += dir * _movementSpeed * Time.fixedDeltaTime);
    }

    public void Rotation(float xAxis, float yAxis)
    {
        _mouseX += xAxis * _mouseSensitivity * Time.deltaTime;

        if (_mouseX >= 360 || _mouseX <= -360)
        {
            _mouseX -= 360 * Mathf.Sign(_mouseX);
        }

        yAxis *= _mouseSensitivity * Time.deltaTime;

        _pj.transform.rotation = Quaternion.Euler(0, _mouseX, 0);
        _cam?.Rotate(_mouseX, yAxis);
    }

    public void Dash(float xAxis, float zAxis)
    {
        Vector3 dir = (_pj.transform.right * xAxis + _pj.transform.forward * zAxis).normalized;

        Vector3 forceToApply = Vector3.zero;

        if(xAxis != 0 || zAxis != 0)
        {
            forceToApply = dir * _dashForce +_pj.transform.up * _dashUpwardForce;
        }
        else
        {
            forceToApply = _pj.transform.forward * _dashForce;
        }

        Debug.Log("Dash");
        Debug.Log("fuerzaAplicada: " + forceToApply);

        _pj.dashing = true;
        _rb.AddForce(forceToApply, ForceMode.Impulse);
        


    }



    public void Jump()
    {
        
        if (0 <= _jumpsRemaining)
        {
            _rb.AddForce(_pj.transform.up * _jumpHeight, ForceMode.Impulse);
            _jumpsRemaining -= 1;
            
               
        }
        Debug.Log(_jumpsRemaining);
    }
    
    public void GroundedState()
    {
        float groundCheckDistance = 1.1f;
        RaycastHit hit;
        if (Physics.Raycast(_pj.transform.position, -_pj.transform.up, out hit, groundCheckDistance))
        {
            _pj.grounded = true;
            _jumpsRemaining = _maxJumpsCount;

            
        }
        else
            _pj.grounded = false;
    }
}
