
using UnityEngine;
using FSM;

public abstract class EnemigoBase : Entity
{
    protected FiniteStateMachine _fsm;

    [SerializeField] protected float _maxVelocity;
    [SerializeField] protected float _maxForce;
    [SerializeField] protected float _viewRadius;
    [SerializeField] protected float _viewAngle;
    [SerializeField] protected LayerMask _wallLayer;

    [SerializeField] protected PuntosDebiles[] _puntosDebiles;

    //Aca instancias el factory por ejemplo:
    //var p = EnemigoVoladorFactory.Instance.pool.GetObject();
    //p.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.rotation.normalized);
    public abstract void SpawnEnemy(Transform spawnPoint);

}
