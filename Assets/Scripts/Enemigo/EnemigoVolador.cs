using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class EnemigoVolador : EnemigoBase, IFreezed, IGridEntity
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

    private Vector3 _position;
    public static SpatialGrid _spatialGrid;

    public static void InitializeGrid()
    {
        _spatialGrid = FindObjectOfType<SpatialGrid>();
    }

   public Vector3 Position
    {
        get { return transform.position; }
        set { transform.position = value; OnMove?.Invoke(this); }

    }

    public event Action<IGridEntity> OnMove;

    
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
        EnemigoVoladorFactory.Instance.ReturnEnemy(this);
        GameManager.instance.pj.CambioDeArma();
        _vida = _vidaMax;
    }

    private void Update()
    {
        delegateUpdate.Invoke();
        if(_position != transform.position)
        {
            _position = transform.position;
            OnMove?.Invoke(this);
        }
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
        //_activeTime = 0;
        _damageText.text = "Damage: 0";
        foreach (PuntosDebiles i in _puntosDebiles)
        {
            i.IsActive = false;
            i.Desactivate();
        }

        ActivateWeakestPoint();

        if (!gameObject.activeInHierarchy)
            GameManager.instance.arenaManager.enemigosEnLaArena.Add(this);

        _spatialGrid.Add(this);
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
        else 
        {
            _spatialGrid.Remove(p);
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
        string damageString = _damageHistory
            .Aggregate("", (result, damage) => result + damage.ToString() + ",");

        if (damageString.EndsWith(","))
        {
            damageString = damageString.Substring(0, damageString.Length - 1);
        }
        var damageValues = damageString.Split(',').Select(int.Parse);
        int totalDamage = damageValues.Aggregate(0,(sum,value) => sum + value);
        _damageText.text = "Damage: " + totalDamage.ToString();
    }

    // #3

    public static void DeactivateEnemiesByDistance(FirstPersonPlayer player, IEnumerable<EnemigoVolador> enemies, float distanceThreshold)
    {
       enemies.Aggregate(new List<EnemigoVolador>(),(toUpdate, enemy) =>
       {
           float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);

           if (distanceToPlayer > distanceThreshold)
           {
               toUpdate.Add(enemy);
               EnemigoVoladorFactory.Instance.ReturnEnemy(enemy);
               enemy.gameObject.transform.GetChild(0).gameObject.SetActive(false);

           }
           else if(distanceToPlayer < distanceThreshold && !enemy.gameObject.activeSelf)
           {
              toUpdate.Add(enemy);
              enemy.gameObject.transform.GetChild(0).gameObject.SetActive(true);

           }
           return toUpdate;
       });
    }

    public static void FuseEnemiesInRange(Vector3 position, float cellSize)
    {
       
        var cellPosition = _spatialGrid.GetPositionInGrid(position);
        if(_spatialGrid.IsInsideGrid(cellPosition))
        {
            var enemiesInCell = _spatialGrid.GetEntitiesInCell(cellPosition).OfType<EnemigoVolador>().ToList();
            if(enemiesInCell.Count>1)
            {
                int fusionCount = 0;
                while (enemiesInCell.Count > 1 && fusionCount<2)
                {
                    var fusedEnemy = enemiesInCell.Aggregate((currentMax, next) => currentMax._vida < next._vida ? currentMax : next);
                    var enemyToFuse= enemiesInCell.FirstOrDefault(enemy => enemy != fusedEnemy);

                    fusedEnemy._vida += enemyToFuse._vida;
                    _spatialGrid.Remove(enemyToFuse);
                    EnemigoVoladorFactory.Instance.ReturnEnemy(enemyToFuse);
                    enemiesInCell.Remove(enemyToFuse);
                    fusionCount++;
                }

                if(enemiesInCell.Count > 0)
                {
                    var remainingFusedEnemy = enemiesInCell[0];
                    _spatialGrid.Remove(remainingFusedEnemy);
                    _spatialGrid.Add(remainingFusedEnemy);
                }
             
            }

        }
    }
}
