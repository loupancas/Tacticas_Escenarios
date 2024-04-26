using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoVolador : Entity, IFreezed
{
    FSM _fsm;

    public delegate void DelegateUpdate();
    public DelegateUpdate delegateUpdate;

    [SerializeField] float _cdShot;
    [SerializeField] float _maxVelocity;
    [SerializeField] float _maxForce;
    [SerializeField] float _viewRadius;
    [SerializeField] float _viewAngle;

    [Header("Components")]
    [SerializeField] PuntosDebiles[] _puntosDebiles;
    [SerializeField] ProyectilesBase _proyectil;
    [SerializeField] Transform _spawnBullet;
    [SerializeField] LayerMask _wallLayer;
    
    public void Awake()
    {
        
    }
    public void Start()
    {
        delegateUpdate = NormalUpdate;
        GameManager.instance.pj.theWorld += StoppedTime;

        _vida = _vidaMax;
        int NumeroRandom = Random.Range(0, _puntosDebiles.Length);
        print(NumeroRandom);
        _puntosDebiles[NumeroRandom].IsActive = true;
        _puntosDebiles[NumeroRandom].Activate();

        _fsm = new FSM();

        _fsm.CreateState("Attack", new EnemigoVoladorAttack(_fsm, _proyectil, transform, _spawnBullet, _wallLayer, _viewRadius, _viewAngle, _cdShot));
        _fsm.CreateState("Lost view", new EnemigoVoladorLostView(_fsm, transform, _wallLayer, _viewRadius, _viewAngle));
        _fsm.CreateState("Movement", new EnemigoVoladorMovimiento(_fsm, transform, _maxVelocity, _maxForce, _viewRadius, _viewAngle, _wallLayer));

        _fsm.ChangeState("Movement");
    }
    public override void Morir()
    {
        print("Mori xd");
        FirstPersonPlayer.instance.CambioDeArma();
    }

    private void Update()
    {
        delegateUpdate.Invoke();
    }

    public void NormalUpdate()
    {
        _fsm.Execute();
    }

    public void Freezed()
    {

    }

    public void StoppedTime()
    {
        StartCoroutine(StopTime());
    }
    public IEnumerator StopTime()
    {
        delegateUpdate = Freezed;
        yield return new WaitForSeconds(3);
        delegateUpdate = NormalUpdate;
    }
}
