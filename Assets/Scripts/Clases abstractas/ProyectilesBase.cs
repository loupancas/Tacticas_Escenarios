using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProyectilesBase : MonoBehaviour
{
    [SerializeField] protected float _speed, _maxDistance, _currentDistance = 0f;
    [SerializeField] protected int _dmg;
    protected float _modifiedSpeed;
    protected int _modifiedDmg;
    public abstract void SpawnProyectile(Transform spawnPoint);
    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
    public void SetDmg(int dmg)
    {
        _dmg = dmg;
    }

    public void SetDistance(float distance)
    {
        _maxDistance = distance;
    }

}
