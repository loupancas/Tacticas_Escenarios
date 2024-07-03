using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmFour : WeaponBase
{
    public override void Fases(int fase)
    {
        switch(fase)
        {
            case 0:
                _modifiedDmg = dmg;
                _modifiedCooldown = shotCooldown;
                break;
            case 1:
                _modifiedCooldown = 0.07f;
                _modifiedDmg = 7;
                break;
            case 2:
                _modifiedCooldown = 0.068f;
                _modifiedDmg = 7;
                break;
            case 3:
                _modifiedDmg = 10;
                break;
                
        }
        
    }

    protected override void FireBehaviour()
    {
        _ray = new Ray(_shotTransform.position, _shotTransform.forward);
        
        if(Physics.Raycast(_ray, out _rayHit, _shootableLayers))
        {
            print("Detecto m4");
            _rayHit.collider.GetComponent<EnemigoVolador>()?.TakeDamage(_modifiedDmg);
            _rayHit.collider.GetComponent<EnemigoVolador>()?.AddDamage(_modifiedDmg);
            _rayHit.collider.GetComponent<PuntosDebiles>()?.OnHit(_modifiedDmg);
        }
    }
}
