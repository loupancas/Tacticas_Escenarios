using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoVoladorAttack : IState
{
    FSM _fsm;
    ProyectilesBase _proyectil;
    EnemigoBase _me;
    List<EnemigoBase> _boids;
    Transform _transform;
    Transform _bulletSpawn;
    LayerMask _maskPlayer;

    float _viewRadius;
    float _viewAngle;
    float _cdShot;
    float _currCdShot;

    public EnemigoVoladorAttack(FSM fsm, ProyectilesBase proyectil, Transform bulletSpawn, LayerMask maskPlayer, float viewRadius, float viewAngle, float cdShot, EnemigoBase me)
    {
        _fsm = fsm;
        _proyectil = proyectil;
        _transform = me.transform;
        _bulletSpawn = bulletSpawn;
        _maskPlayer = maskPlayer;
        _viewAngle = viewAngle;
        _viewRadius = viewRadius;
        _cdShot = cdShot;
        _me = me;
        _boids = GameManager.instance.arenaManager.enemigosEnLaArena;
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

        



        if (InFOV(GameManager.instance.pj.transform))
        {

            Vector3 pos = new Vector3(GameManager.instance.pj.transform.position.x, GameManager.instance.pj.transform.position.y, GameManager.instance.pj.transform.position.z);
            _transform.LookAt(pos);
            Debug.Log("Detectado");
            if (_currCdShot > _cdShot)
            {
            _proyectil.SpawnProyectile(_bulletSpawn);
            _currCdShot = 0;
            }        
        }
        else
        {
            _fsm.ChangeState("Movement");
        }
        
        

        
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
