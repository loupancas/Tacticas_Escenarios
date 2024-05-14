using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class player_Inputs 
{
    
    float _xAxis, _zAxis, _inputMouseX, _inputMouseY, _currTimeMeleeAttack, _currTimeTimeStop;
    int _dashsRemaining;
    player_Movement _movement;
    WeaponBase _equippedWeapon;
    AttackMelee _attackMelee;
    FirstPersonPlayer _pj;
    TextoActualizable _textDashCounts;
    ModifierStat _baseStatsPlayer;
    Controles _controles;
    public player_Inputs(player_Movement movement, WeaponBase equippedWeapon, FirstPersonPlayer pj, TextoActualizable text, ModifierStat baseStatsPlayer, Controles controles, AttackMelee attackMelee)
    {
        _movement = movement;
        _equippedWeapon = equippedWeapon;
        _pj = pj;
        _textDashCounts = text;
        _baseStatsPlayer = baseStatsPlayer;
        _controles = controles;
        _attackMelee = attackMelee;
    }

    public void TimeStop()
    {
        _currTimeTimeStop += Time.deltaTime;
        if(Input.GetKey(_controles.stopTime) && _baseStatsPlayer.StatResultado.cooldownFreeze < _currTimeTimeStop)
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
                if (Input.GetKey(_controles.fireKey) && _equippedWeapon.CanShoot)
                {
                    _equippedWeapon.Fire();
                }
                break;
            case WeaponBase.ShootType.SemiAutomatic:
                if (Input.GetKeyDown(_controles.fireKey) && _equippedWeapon.CanShoot)
                {
                    _equippedWeapon.Fire();
                }
                break;
        }

    }

    public void MeleeAttack()
    {
        _currTimeMeleeAttack += Time.deltaTime;
        if(Input.GetKeyDown(_controles.meleeKey) && _currTimeMeleeAttack > _baseStatsPlayer.StatResultado.cooldownMeleeAttack)
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
        if (Input.GetKeyDown(_controles.dashKey) && (0 < _dashsRemaining))
        {
            _movement.Dash(_xAxis, _zAxis);
            _dashsRemaining -= 1;
        }
        
        if(_pj.grounded && _dashsRemaining <= (_baseStatsPlayer.StatResultado.maxDashsCount - 1) && !_pj.dashing)
        {
            _dashsRemaining = _baseStatsPlayer.StatResultado.maxDashsCount;
            Debug.Log("Carga del dash");
            _textDashCounts.UpdateHUD(_dashsRemaining, _baseStatsPlayer.StatResultado.maxDashsCount, "Dash");
        }
        Debug.Log("Dashes restantes: " + _dashsRemaining);
        _textDashCounts.UpdateHUD(_dashsRemaining, _baseStatsPlayer.StatResultado.maxDashsCount, "Dash");
    }

    public void Jump()
    {
        
        if (Input.GetKeyDown(_controles.jumpKey))
        {
            _movement.Jump();
        }
        
    }
}
