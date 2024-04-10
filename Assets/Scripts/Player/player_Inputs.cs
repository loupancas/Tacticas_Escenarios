using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Inputs 
{
    KeyCode _fireKey, _meleeKey, _stopTimeKey, _dashKey, _jumpKey;
    float _xAxis, _zAxis, _inputMouseX, _inputMouseY, _cooldownMeleeAttack, _currTimeMeleeAttack, _cooldownTimeStop, _currTimeTimeStop;
    int _maxDashCount;
    int _dashsRemaining;
    player_Movement _movement;
    WeaponBase _equippedWeapon;
    AttackMelee _attackMelee;
    FirstPersonPlayer _pj;
    
    public player_Inputs (KeyCode fireKey, float xAxis, float zAxis, float inputMouseX, float inputMouseY, player_Movement movement, WeaponBase equippedWeapon, AttackMelee attackMelee, KeyCode meleeKey, KeyCode stopTimeKey, float cooldownTimeStop, KeyCode dashKey, KeyCode jumpKey, FirstPersonPlayer pj, int maxDashCount)
    {
        _fireKey = fireKey;
        _xAxis = xAxis;
        _zAxis = zAxis;
        _inputMouseX = inputMouseX;
        _inputMouseY = inputMouseY;
        _movement = movement;
        _equippedWeapon = equippedWeapon;
        _attackMelee = attackMelee;
        _meleeKey = meleeKey;
        _stopTimeKey = stopTimeKey;
        _cooldownTimeStop = cooldownTimeStop;
        _dashKey = dashKey;
        _jumpKey = jumpKey;
        _pj = pj;
        _maxDashCount = maxDashCount;
    }

    public void TimeStop()
    {
        _currTimeTimeStop += Time.deltaTime;
        if(Input.GetKey(_stopTimeKey) && _cooldownTimeStop < _currTimeTimeStop)
        {
            FirstPersonPlayer.instance.theWorld.Invoke();
           
            _currTimeTimeStop = 0;
        }
    }
    public void Rotation()
    {
        _inputMouseX = Input.GetAxisRaw("Mouse X");
        _inputMouseY = Input.GetAxisRaw("Mouse Y");
        if (_inputMouseX != 0 || _inputMouseY != 0)
        {
            _movement.Rotation(_inputMouseX, _inputMouseY);
        }

        switch (_equippedWeapon.gunType)
        {
            case WeaponBase.ShootType.Automatic:
                if (Input.GetKey(_fireKey) && _equippedWeapon.CanShoot)
                {
                    _equippedWeapon.Fire();
                }
                break;
            case WeaponBase.ShootType.SemiAutomatic:
                if (Input.GetKeyDown(_fireKey) && _equippedWeapon.CanShoot)
                {
                    _equippedWeapon.Fire();
                }
                break;
        }

    }

    public void MeleeAttack()
    {
        _currTimeMeleeAttack += Time.deltaTime;
        if(Input.GetKeyDown(_meleeKey) && _currTimeMeleeAttack > _cooldownMeleeAttack)
        {
            _attackMelee.gameObject.SetActive(true);
            _attackMelee.StartCoroutine(_attackMelee.SpawnTime());
        }
    }

    public void UpdateWeapon(WeaponBase Arma)
    {
        Debug.Log(Arma);

        _equippedWeapon = Arma;
    }
  

    public void Control()
    {
        _xAxis = Input.GetAxisRaw("Horizontal");
        _zAxis = Input.GetAxisRaw("Vertical");
        if (_xAxis != 0 || _zAxis != 0)
        {
            _movement.Movement(_xAxis, _zAxis);
        }
    }
    
    public void Dash()
    {
        _xAxis = Input.GetAxisRaw("Horizontal");
        _zAxis = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(_dashKey) && 0 <= _dashsRemaining)
        {
            _movement.Dash(_zAxis, _xAxis);
            _dashsRemaining -= 1;
        }
        
        if(_pj.dashing! && _pj.grounded && _dashsRemaining == 0)
        {
            _dashsRemaining = _maxDashCount;
        }
    }

    public void Jump()
    {
        
        if (Input.GetKeyDown(_jumpKey))
        {
            Debug.Log("Espacio apretado");
            _movement.Jump();
        }
        
    }
}
