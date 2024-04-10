using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProyectilesBase : MonoBehaviour
{
    [SerializeField] protected float _speed, _maxDistance, _currentDistance = 0f;
    [SerializeField] protected int _dmg;

    public abstract void SpawnProyectile(Transform spawnPoint);

}
