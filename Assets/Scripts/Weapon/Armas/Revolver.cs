using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : WeaponBase
{
    bool _IsFase3On = false;
    public override void Fases(int fase)
    {
        switch (fase)
        {
            case 0:
                _modifiedDmg = dmg;
                _modifiedCooldown = shotCooldown;
                _IsFase3On = false;
                break;
            case 1:
                _modifiedCooldown = 0.7f;
                _modifiedDmg = 50;
                _IsFase3On = false;
                break;
            case 2:
                _modifiedCooldown = 0.1f;
                _modifiedDmg = 75;
                _IsFase3On = false;
                break;
            case 3:
                _modifiedCooldown = 0.1f;
                _modifiedDmg = 75;
                _gunType = ShootType.Automatic;
                _IsFase3On = true;
                break;

        }
    }

    protected override void FireBehaviour()
    {
        _ray = new Ray(_shotTransform.position, _shotTransform.forward);

        if (Physics.Raycast(_ray, out _rayHit, _shootableLayers))
        {
            print("Disparo con revolver");
            _rayHit.collider.GetComponent<EnemigoVolador>()?.AddDamage(_modifiedDmg);
            _rayHit.collider.GetComponent<EnemigoVolador>()?.TakeDamage(_modifiedDmg);
            _rayHit.collider.GetComponent<PuntosDebiles>()?.OnHit(_modifiedDmg);
            _rayHit.collider.GetComponent<Projectile>()?.DevolverBala();

        }
    }
}
