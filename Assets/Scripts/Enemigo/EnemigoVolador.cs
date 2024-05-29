using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoVolador : EnemigoBase, IFreezed
{
    

    public delegate void DelegateUpdate();
    public DelegateUpdate delegateUpdate;
    [Header("Stats")]
    [SerializeField] float _cdShot;
    

    [Header("Components")]
    
    [SerializeField] ProyectilesBase _proyectil;
    [SerializeField] Transform _spawnBullet;

    CountdownTimer _Freezetime;
    public void Awake()
    {
        
    }
    public void Start()
    {
        delegateUpdate = NormalUpdate;
        GameManager.instance.pj.theWorld += StoppedTime;
        _Freezetime = new CountdownTimer(3);
        _Freezetime.OnTimerStop = BackToNormal;
        //GameManager.instance.arenaManager.enemigosEnLaArena.Add(this);

        _vida = _vidaMax;
        //int NumeroRandom = Random.Range(0, _puntosDebiles.Length);
        //print(NumeroRandom);
        //_puntosDebiles[NumeroRandom].IsActive = true;
        //_puntosDebiles[NumeroRandom].Activate();

        _fsm = new FSM();

        _fsm.CreateState("Attack", new EnemigoVoladorAttack(_fsm, _proyectil, _spawnBullet, _wallLayer, _viewRadius, _viewAngle, _cdShot, this));
        _fsm.CreateState("Lost view", new EnemigoVoladorLostView(_fsm, transform, _wallLayer, _viewRadius, _viewAngle));
        _fsm.CreateState("Movement", new EnemigoVoladorMovimiento(_fsm, _maxVelocity, _maxForce, _viewRadius, _viewAngle, _wallLayer, this));

        _fsm.ChangeState("Movement");
    }
    public override void Morir()
    {
        GameManager.instance.arenaManager.enemigosEnLaArena.Remove(this);
        EnemigoVoladorFactory.Instance.ReturnProjectile(this);
        GameManager.instance.pj.CambioDeArma();
        _vida = _vidaMax;
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
        _Freezetime.Tick(Time.deltaTime);
    }
    public void StoppedTime()
    {
        delegateUpdate = Freezed;
        _Freezetime.Start();
    }
    public void BackToNormal()
    {
        delegateUpdate = NormalUpdate;
    }
    public override void SpawnEnemy(Transform spawnPoint)
    {
        var p = EnemigoVoladorFactory.Instance.pool.GetObject();
        p.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.rotation.normalized);
        Debug.Log("Disparo proyectil");
        //GameManager.instance.arenaManager.enemigosEnLaArena.Add(this);
    }

    private void Reset()
    {
        _vida = _vidaMax;
        foreach(PuntosDebiles i in _puntosDebiles)
        {
            i.IsActive = false;
            i.Desactivate();
        }
        int NumeroRandom = Random.Range(0, _puntosDebiles.Length);
        print(NumeroRandom + " Reinicio");
        _puntosDebiles[NumeroRandom].IsActive = true;
        _puntosDebiles[NumeroRandom].Activate();

        if (!gameObject.activeInHierarchy) 
            GameManager.instance.arenaManager.enemigosEnLaArena.Add(this);
    }

    public static void TurnOnOff(EnemigoVolador p, bool active = true)
    {
        if (active)
        {
            p.Reset();
        }
        p.gameObject.SetActive(active);
    }

    
}
