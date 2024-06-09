using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FSM;

public class EnemigoVolador : EnemigoBase, IFreezed
{
    

    public delegate void DelegateUpdate();
    public DelegateUpdate delegateUpdate;
    [Header("Stats")]
    public float cdShot;
    [SerializeField] float _distanceToAttack;
    [SerializeField] float _distanceToSeparation;

    [Header("Components")]
    [SerializeField] TrackEnemigoVolador trackState;
    [SerializeField] SeparationEnemigoVolador separationState;
    [SerializeField] AttackEnemigoVolador attackState;



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

        _vida = _vidaMax;
        
        var weakestPoint = _puntosDebiles.Aggregate((currentWeakest,next) => next.resistance < currentWeakest.resistance ? next : currentWeakest);
        weakestPoint.IsActive = true;
        weakestPoint.Activate();

        _fsm = new FiniteStateMachine(trackState, StartCoroutine);

        _fsm.AddTransition(StateTransitions.ToSeparation, trackState, separationState);
        _fsm.AddTransition(StateTransitions.ToSeparation, attackState, separationState);
        _fsm.AddTransition(StateTransitions.ToPersuit, separationState, trackState);
        _fsm.AddTransition(StateTransitions.ToPersuit, attackState, trackState);
        _fsm.AddTransition(StateTransitions.ToAttack, attackState, trackState);

        _fsm.Active = true;

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

    void Reset()
    {
        _vida = _vidaMax;
        foreach(PuntosDebiles i in _puntosDebiles)
        {
            i.IsActive = false;
            i.Desactivate();
        }
        var weakestPoint = _puntosDebiles.Aggregate((currentWeakest, next) => next.resistance < currentWeakest.resistance ? next : currentWeakest);
        weakestPoint.IsActive = true;
        weakestPoint.Activate();

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

    public bool IsAttackDistance()
    {
        return Vector3.Distance(GameManager.instance.pj.transform.position, transform.position) >= _distanceToAttack;
    }

    public bool IsSeparationDistance()
    {
        foreach (EnemigoBase a in GameManager.instance.arenaManager.enemigosEnLaArena)
        {
            if (a == this)
                continue;

            return Vector3.Distance(a.transform.position, transform.position) >= _distanceToAttack;
        }

        return this;
    }

}
