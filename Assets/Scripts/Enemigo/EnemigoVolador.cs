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
    
    
    public void Awake()
    {
        
    }
    public void Start()
    {
        delegateUpdate = NormalUpdate;
        GameManager.instance.pj.theWorld += StoppedTime;
        //GameManager.instance.arenaManager.enemigosEnLaArena.Add(this);

        //_vida = _vidaMax;
        //int NumeroRandom = Random.Range(0, _puntosDebiles.Length);
        //print(NumeroRandom);
        //_puntosDebiles[NumeroRandom].IsActive = true;
        //_puntosDebiles[NumeroRandom].Activate();

        _fsm = new FSM();

        _fsm.CreateState("Attack", new EnemigoVoladorAttack(_fsm, _proyectil, transform, _spawnBullet, _wallLayer, _viewRadius, _viewAngle, _cdShot));
        _fsm.CreateState("Lost view", new EnemigoVoladorLostView(_fsm, transform, _wallLayer, _viewRadius, _viewAngle));
        _fsm.CreateState("Movement", new EnemigoVoladorMovimiento(_fsm, transform, _maxVelocity, _maxForce, _viewRadius, _viewAngle, _wallLayer));

        _fsm.ChangeState("Movement");
    }
    public override void Morir()
    {
        print("Mori xd");
        GameManager.instance.arenaManager.enemigosEnLaArena.Remove(this);
        EnemigoVoladorFactory.Instance.ReturnProjectile(this);
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

    public override void SpawnEnemy(Transform spawnPoint)
    {
        var p = EnemigoVoladorFactory.Instance.pool.GetObject();
        p.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.rotation.normalized);
        Debug.Log("Disparo proyectil");
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
