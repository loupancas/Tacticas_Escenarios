using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : WeaponBase
{
    protected override void FireBehaviour()
    {
        _ray = new Ray(_shotTransform.position, _shotTransform.forward);

        if (Physics.Raycast(_ray, out _rayHit, _shootableLayers))
        {
            print("Detecto algo");
            _rayHit.collider.GetComponent<Enemigo>()?.TakeDamage(_dmg);
            _rayHit.collider.GetComponent<PuntosDebiles>()?.OnHit(_dmg);
        }
    }
}
