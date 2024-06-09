using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

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
    [SerializeField] private TextMeshProUGUI _damageText;

    public void Awake()
    {

    }
    public void Start()
    {
        delegateUpdate = NormalUpdate;
        GameManager.instance.pj.theWorld += StoppedTime;
        _Freezetime = new CountdownTimer(3);
        _Freezetime.OnTimerStop = BackToNormal;
        _damageText.text = "Damage: 0";

        _vida = _vidaMax;

        ActivateWeakestPoint();

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
        _activeTime += Time.deltaTime;
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
    }

    public void Reset()
    {
        _vida = _vidaMax;
        _activeTime = 0;
        _damageText.text = "Damage: 0";
        foreach (PuntosDebiles i in _puntosDebiles)
        {
            i.IsActive = false;
            i.Desactivate();
        }

        ActivateWeakestPoint();

        if (!gameObject.activeInHierarchy)
            GameManager.instance.arenaManager.enemigosEnLaArena.Add(this);
    }

    private void ActivateWeakestPoint()
    {
        var weakestPoint = _puntosDebiles
           .Aggregate((currentWeakest, next) => next.resistance < currentWeakest.resistance ? next : currentWeakest);

        weakestPoint.IsActive = true;
        weakestPoint.Activate();
    }

    public static void TurnOnOff(EnemigoVolador p, bool active = true)
    {
        if (active)
        {
            p.Reset();
        }
        p.gameObject.SetActive(active);
    }

    // #2

    private List<int> _damageHistory = new List<int>();

    public void AddDamage(int damage)
    {
        _damageHistory.Add(damage);
        UpdateDamageUI();
    }

    private void UpdateDamageUI()
    {
        //string damageString = _damageHistory
        //    .Aggregate("Damage: ", (result, damage) => result + damage.ToString() + ", ");
        int totalDamage = _damageHistory.Aggregate(0, (sum, damage) => sum + damage);

        // if (damageString.EndsWith(", "))
        // {
        //     damageString = damageString.Substring(0, damageString.Length - 2);
        // }

        _damageText.text = "Damage: " + totalDamage.ToString();
    }

    // #3

    private float _activeTime;
    public static void DeactivateOldEnemies(IEnumerable<EnemigoVolador> enemigos, float timeThreshold) // desde un manager o poder desactivar enemigos que llevan determinado tiempo activos
    {
        enemigos.Aggregate(new List<EnemigoVolador>(), (toDeactivate, enemigo) =>
        {
            if (enemigo._activeTime > timeThreshold)
            {
                toDeactivate.Add(enemigo);
                enemigo.gameObject.SetActive(false);
            }
            return toDeactivate;
        });
    }
}
