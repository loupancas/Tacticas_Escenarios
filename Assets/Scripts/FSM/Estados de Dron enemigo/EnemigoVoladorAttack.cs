using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoVoladorAttack : IState
{
    FSM _fsm;
    ProyectilesBase _proyectil;
    Transform _transform;
    Transform _bulletSpawn;
    LayerMask _maskPlayer;

    float _viewRadius;
    float _viewAngle;
    float _cdShot;
    float _currCdShot;

    public EnemigoVoladorAttack(FSM fsm, ProyectilesBase proyectil, Transform transform, Transform bulletSpawn, LayerMask maskPlayer, float viewRadius, float viewAngle, float cdShot)
    {
        _fsm = fsm;
        _proyectil = proyectil;
        _transform = transform;
        _bulletSpawn = bulletSpawn;
        _maskPlayer = maskPlayer;
        _viewAngle = viewAngle;
        _viewRadius = viewRadius;
        _cdShot = cdShot;
       
    }
    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        _currCdShot += Time.deltaTime;

        Vector3 pos = new Vector3(GameManager.instance.pj.transform.position.x, GameManager.instance.pj.transform.position.y, GameManager.instance.pj.transform.position.z);
        _transform.LookAt(pos, _transform.forward);

        if (_currCdShot > _cdShot)
        {
            _proyectil.SpawnProyectile(_bulletSpawn);
            _currCdShot = 0;
        }

        //if (InFOV(GameManager.instance.pj.transform))
        //{
           
        //}
                
        
    }

    
    protected bool InLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, _maskPlayer);
    }

    public bool InFOV(Transform obj)
    {
        var dir = obj.position - _transform.position;

        if (dir.magnitude < _viewRadius)
        {
            if (Vector3.Angle(_transform.forward, dir) <= _viewAngle * 0.5f)
            {
                return InLineOfSight(_transform.position, obj.position);
            }
        }

        return false;
    }
}
