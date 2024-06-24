using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileGun : WeaponBase
{
    [SerializeField] ProyectilesBase proyectil;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] float _proyectileSpeed;
    public override void Fases(int fase)
    {
        switch(fase)
        {
            case 0:
                proyectil.SetDistance(10);
                proyectil.SetDmg(dmg);
                proyectil.SetSpeed(_proyectileSpeed);
                _modifiedCooldown = shotCooldown;
                break;
            case 1:
                proyectil.SetDistance(10);
                proyectil.SetDmg(dmg + 5);
                proyectil.SetSpeed(_proyectileSpeed + 5);
                _modifiedCooldown = shotCooldown;
                break;
            case 2:
                proyectil.SetDistance(10);
                proyectil.SetDmg(dmg + 15);
                proyectil.SetSpeed(_proyectileSpeed + 10);
                _modifiedCooldown = 0.5f;
                break;
            case 3:
                proyectil.SetDistance(10);
                proyectil.SetDmg(dmg + 20);
                proyectil.SetSpeed(_proyectileSpeed + 20);
                _modifiedCooldown = 0.1f;
                break;
        }
    }

    protected override void FireBehaviour()
    {
        print("Disparo con Proyectil Gun");
        proyectil.SpawnProyectile(_spawnPoint);
    }
}
