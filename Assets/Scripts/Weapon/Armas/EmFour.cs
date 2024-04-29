using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmFour : WeaponBase
{
    protected override void FireBehaviour()
    {
        _ray = new Ray(_shotTransform.position, _shotTransform.forward);
        
        if(Physics.Raycast(_ray, out _rayHit, _shootableLayers))
        {
            
            _rayHit.collider.GetComponent<EnemigoVolador>()?.TakeDamage(dmg);
            _rayHit.collider.GetComponent<PuntosDebiles>()?.OnHit(dmg);
        }
    }
}
