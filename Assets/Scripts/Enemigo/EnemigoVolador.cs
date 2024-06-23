using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FSM;
<<<<<<< Updated upstream
using System;
using TMPro;
=======
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream

    [SerializeField] private TextMeshProUGUI _damageText;

    private Vector3 _position;
    public static SpatialGrid _spatialGrid;

    public static void InitializeGrid()
    {
        _spatialGrid = FindObjectOfType<SpatialGrid>(); // Busca el componente en la escena
    }

    public Vector3 Position  // Implementación de IGridEntity
    {
        get { return transform.position; }
        set
        {
            transform.position = value;
            OnMove?.Invoke(this);
        }
    }

    public event Action<IGridEntity> OnMove;

=======
    public void Awake()
    {
        
    }
>>>>>>> Stashed changes
    public void Start()
    {
        delegateUpdate = NormalUpdate;
        GameManager.instance.pj.theWorld += StoppedTime;
        _Freezetime = new CountdownTimer(3);
        _Freezetime.OnTimerStop = BackToNormal;
<<<<<<< Updated upstream
        _damageText.text = "Damage: 0";
=======

>>>>>>> Stashed changes
        _vida = _vidaMax;

        attackState = new AttackEnemigoVolador(this, _proyectil, _spawnBullet);
        trackState = new TrackEnemigoVolador(this, _maxVelocity, _maxForce);
        separationState = new SeparationEnemigoVolador(this, _maxVelocity, _maxForce);

<<<<<<< Updated upstream
        attackState = new AttackEnemigoVolador(this, _proyectil, _spawnBullet);
        trackState = new TrackEnemigoVolador(this, _maxVelocity, _maxForce);
        separationState = new SeparationEnemigoVolador(this, _maxVelocity, _maxForce);       
=======
        var weakestPoint = _puntosDebiles.Aggregate((currentWeakest,next) => next.resistance < currentWeakest.resistance ? next : currentWeakest);
        weakestPoint.IsActive = true;
        weakestPoint.Activate();
>>>>>>> Stashed changes

        _fsm = new FiniteStateMachine(trackState, StartCoroutine);

        _fsm.AddTransition(StateTransitions.ToSeparation, trackState, separationState);
        _fsm.AddTransition(StateTransitions.ToSeparation, attackState, separationState);
        _fsm.AddTransition(StateTransitions.ToPersuit, separationState, trackState);
        _fsm.AddTransition(StateTransitions.ToPersuit, attackState, trackState);
        _fsm.AddTransition(StateTransitions.ToAttack, trackState, attackState);

        _fsm.Active = true;

    }
    public override void Morir()
    {
        GameManager.instance.arenaManager.enemigosEnLaArena.Remove(this);
        EnemigoVoladorFactory.Instance.ReturnProjectile(this);
        GameManager.instance.pj.CambioDeArma();
        _vida = _vidaMax;

<<<<<<< Updated upstream
    }    
=======
    }
>>>>>>> Stashed changes

    

    private void Update()
    {
        delegateUpdate.Invoke();
<<<<<<< Updated upstream

        if (_position != transform.position)  // Añadido
        {
            _position = transform.position;
            OnMove?.Invoke(this);
        }
    }    
=======
    }

    public void NormalUpdate()
    {
        
    }
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
        _damageText.text = "Damage: 0";

        foreach (PuntosDebiles i in _puntosDebiles)
=======
        foreach(PuntosDebiles i in _puntosDebiles)
>>>>>>> Stashed changes
        {
            i.IsActive = false;
            i.Desactivate();
        }
<<<<<<< Updated upstream
        
        ActivateWeakestPoint();

        if (!gameObject.activeInHierarchy) 
            GameManager.instance.arenaManager.enemigosEnLaArena.Add(this);
    }

    public bool IsAttackDistance()
    {
        return Vector3.Distance(GameManager.instance.pj.transform.position, transform.position) <= _distanceToAttack;
    }

    public bool IsSeparationDistance()
    {
        foreach (EnemigoBase a in GameManager.instance.arenaManager.enemigosEnLaArena)
        {
            if (a == this)
                continue;

            return Vector3.Distance(a.transform.position, transform.position) <= _distanceToSeparation;
        }

        return this;
    }

    private void ActivateWeakestPoint()
    {
        var weakestPoint = _puntosDebiles
           .Aggregate((currentWeakest, next) => next.resistance < currentWeakest.resistance ? next : currentWeakest);

=======
        var weakestPoint = _puntosDebiles.Aggregate((currentWeakest, next) => next.resistance < currentWeakest.resistance ? next : currentWeakest);
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        else
        {
            _spatialGrid.Remove(p);
        }
        p.gameObject.SetActive(active);
    }

    private List<int> _damageHistory = new List<int>();

    public void AddDamage(int damage)
=======
        p.gameObject.SetActive(active);
    }

    public bool IsAttackDistance()
>>>>>>> Stashed changes
    {
        return Vector3.Distance(GameManager.instance.pj.transform.position, transform.position) <= _distanceToAttack;
    }

    public bool IsSeparationDistance()
    {
        foreach (EnemigoBase a in GameManager.instance.arenaManager.enemigosEnLaArena)
        {
            if (a == this)
                continue;

            return Vector3.Distance(a.transform.position, transform.position) <= _distanceToSeparation;
        }
<<<<<<< Updated upstream
        var damageValues = damageString.Split(',').Select(int.Parse);
        int totalDamage = damageValues.Aggregate(0, (sum, value) => sum + value);
        _damageText.text = "Damage: " + totalDamage.ToString();
    }

    public static void DeactivateEnemiesByDistance(FirstPersonPlayer player, IEnumerable<EnemigoVolador> enemies, float distanceThreshold)
    {
        enemies.Aggregate(new List<EnemigoVolador>(), (toUpdate, enemy) =>
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, enemy.transform.position);

            if (distanceToPlayer > distanceThreshold)
            {
                toUpdate.Add(enemy);
                EnemigoVoladorFactory.Instance.ReturnEnemy(enemy);
                enemy.gameObject.transform.GetChild(0).gameObject.SetActive(false);

            }
            else if (distanceToPlayer < distanceThreshold && !enemy.gameObject.activeSelf)
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
        if (_spatialGrid.IsInsideGrid(cellPosition))
        {
            var enemiesInCell = _spatialGrid.GetEntitiesInCell(cellPosition).OfType<EnemigoVolador>().ToList();
            if (enemiesInCell.Count > 1)
            {
                int fusionCount = 0;
                while (enemiesInCell.Count > 1 && fusionCount < 2)
                {
                    var fusedEnemy = enemiesInCell.Aggregate((currentMax, next) => next._vida > currentMax._vida ? next : currentMax);
                    var enemyToFuse = enemiesInCell.First(e => e != fusedEnemy);

                    fusedEnemy._vida += enemyToFuse._vida;
                    _spatialGrid.Remove(enemyToFuse);
                    EnemigoVoladorFactory.Instance.ReturnEnemy(enemyToFuse);
                    enemiesInCell.Remove(enemyToFuse);
                    fusionCount++;
                    Debug.Log("Fusion de enemigos");
                }

                if (enemiesInCell.Count > 0) // Check if there is any enemy left in the cell
                {
                    var remainingFusedEnemy = enemiesInCell[0]; // Take the first remaining enemy
                    _spatialGrid.Remove(remainingFusedEnemy);
                    _spatialGrid.Add(remainingFusedEnemy);
                    Debug.Log("Fusion de enemigos 2");
                }
            }
        }
    }

    public void NormalUpdate()
    {
      
    }
=======

        return this;
    }

>>>>>>> Stashed changes
}
