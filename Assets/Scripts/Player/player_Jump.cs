using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Jump 
{
    int _jumpsRemaining;
    ModifierStat _buffs;
    FirstPersonPlayer _pj;
    Rigidbody _rb;
    TextoActualizable _textJumpCount;

    public player_Jump(ModifierStat buffs, FirstPersonPlayer pj, TextoActualizable texto)
    {
        _buffs = buffs;
        _pj = pj;
        _rb = pj.gameObject.GetComponent<Rigidbody>();
        _textJumpCount = texto;
        _textJumpCount.UpdateHUD(_jumpsRemaining, _buffs.StatResultado.maxJumpsCount, "Saltos");
    }

    public void Jump()
    {
        
        if (0 < (_jumpsRemaining - 1))
        {
            _rb.AddForce(_pj.transform.up * _buffs.StatResultado.jumpHeight, ForceMode.Impulse);
            _jumpsRemaining -= 1;
            
        }
        
    }
    public void GroundedState()
    {
        _textJumpCount.UpdateHUD(_jumpsRemaining -1, _buffs.StatResultado.maxJumpsCount -1, "Salto EXTRA");
        float groundCheckDistance = 1.1f;
        RaycastHit hit;
        if (Physics.Raycast(_pj.transform.position, -_pj.transform.up, out hit, groundCheckDistance))
        {
            _pj.grounded = true;
            _jumpsRemaining = _buffs.StatResultado.maxJumpsCount;
        }
        else
        {
            _pj.grounded = false;

        }



        //Debug.Log("Remaining jumps: " + _jumpsRemaining);
    }
}
