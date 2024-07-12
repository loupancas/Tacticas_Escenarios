using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FSM;
using System;
using TMPro;
using UnityEngine.VFX;
public class EnemigoVolador : EnemigoBase, IFreezed, IGridEntity
{
    

    public delegate void DelegateUpdate();
    public DelegateUpdate delegateUpdate;
    [Header("Stats")]
    public float cdShot;
    [SerializeField] float _distanceToAttack;
    [SerializeField] float _distanceToSeparation;
    [SerializeField] bool _IsDead;

    [Header("Components")]
    [SerializeField] TrackEnemigoVolador trackState;
    [SerializeField] SearchEnemigoVolador searchState;
    [SerializeField] AttackEnemigoVolador attackState;
    [SerializeField] LostViewEnemigoVolador lostViewState;
    
    [SerializeField] public VisualEffect _damageParticle;
    [SerializeField] public ParticleSystem _explosionPrefab;

    [SerializeField] ProyectilesBase _proyectil;
    [SerializeField] Transform _spawnBullet;
    private DamageFeedback damageFeedback;


    CountdownTimer _Freezetime;

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
    public override void TakeDamage(int Damage)
    {
        _vida -= Damage;
        _damageParticle.Play();
        damageFeedback.TakeDamage();
        if (_vida <= 0 && !_IsDead) // Cambiado de < a <=
        {
            _IsDead = true;
            // Instanciar el prefab de explosión
            var explosion = Instantiate(_explosionPrefab, transform.position, transform.rotation);
            explosion.Play(); // Reproducir las partículas de la explosión
            GameManager.instance.pj.CambioDeArma();
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            
            // Asegurarse de que el método Morir se ejecute después de un breve delay
            Invoke(nameof(Morir), explosion.main.duration);
        }
    }
    public event Action<IGridEntity> OnMove;
      

    public void Start()
    {
        delegateUpdate = NormalUpdate;
        GameManager.instance.pj.theWorld += StoppedTime;
        _Freezetime = new CountdownTimer(10);
        _Freezetime.OnTimerStop = BackToNormal;
        _damageText.text = "Damage: 0";
        _vida = _vidaMax;

        damageFeedback = GetComponent<DamageFeedback>();

        var weakestPoint = _puntosDebiles.Aggregate((currentWeakest,next) => next.resistance < currentWeakest.resistance ? next : currentWeakest);
        weakestPoint.IsActive = true;
        weakestPoint.Activate();

        //IA2-P3

        _fsm = new FiniteStateMachine(trackState, StartCoroutine);

        _fsm.AddTransition(StateTransitions.ToSearch, attackState, searchState);
        _fsm.AddTransition(StateTransitions.ToSearch, trackState, searchState);

        _fsm.AddTransition(StateTransitions.ToPersuit, searchState, trackState);
        _fsm.AddTransition(StateTransitions.ToPersuit, attackState, trackState);
        _fsm.AddTransition(StateTransitions.ToPersuit, lostViewState, trackState);

        _fsm.AddTransition(StateTransitions.ToAttack, trackState, attackState);
        _fsm.AddTransition(StateTransitions.ToAttack, searchState, attackState);
        _fsm.AddTransition(StateTransitions.ToAttack, searchState, attackState);
        
        _fsm.AddTransition(StateTransitions.ToIdle, searchState, lostViewState);
        

        _fsm.Active = true;

    }
    public override void Morir()
    {
        
        GameManager.instance.arenaManager.enemigosEnLaArena.Remove(this);
        EnemigoVoladorFactory.Instance.ReturnEnemy(this);
        
        _vida = _vidaMax;

    }    

    

    private void Update()
    {
        //print(InLineOfSight(transform.position, GameManager.instance.pj.transform.position));
        delegateUpdate.Invoke();
        if (_position != transform.position)  // Añadido
        {
            _position = transform.position;
            OnMove?.Invoke(this);
        }
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
        _fsm.Active = false;
        _Freezetime.Start();
    }
    public void BackToNormal()
    {
        delegateUpdate = NormalUpdate;
        _fsm.Active = true;
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

    public bool IsAttackDistance()
    {
        return Vector3.Distance(GameManager.instance.pj.transform.position, transform.position) <= _distanceToAttack;
    }

    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, _wallLayer);
    }

    //IA2-TP2 Aggregate identifica el punto el punto más débil y lo activa
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

    private List<int> _damageHistory = new List<int>();

    public void AddDamage(int damage)
    {
        _damageHistory.Add(damage);
        UpdateDamageUI();
    }
    //IA2-TP2 Aggregate calcula el daño total recibido por el enemigo y lo muestra en pantalla
    private void UpdateDamageUI()
    {
        string damageString = _damageHistory
            .Aggregate("", (result, damage) => result + damage.ToString() + ",");

        if (damageString.EndsWith(","))
        {
            damageString = damageString.Substring(0, damageString.Length - 1);
        }
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
    //IA2-TP2 Aggregate fusiona los enemigos que se encuentran en la misma celda, todavia se va a revisar porque hay conflicto con la FSM
    //public static void FuseEnemiesInRange(Vector3 position, float cellSize)
    //{
    //    var cellPosition = _spatialGrid.GetPositionInGrid(position);
    //    if (_spatialGrid.IsInsideGrid(cellPosition))
    //    {
    //        var enemiesInCell = _spatialGrid.GetEntitiesInCell(cellPosition).OfType<EnemigoVolador>().ToList();
    //        if (enemiesInCell.Count > 1)
    //        {
    //            int fusionCount = 0;
    //            while (enemiesInCell.Count > 1 && fusionCount < 2)
    //            {
    //                var fusedEnemy = enemiesInCell.Aggregate((currentMax, next) => next._vida > currentMax._vida ? next : currentMax);
    //                var enemyToFuse = enemiesInCell.First(e => e != fusedEnemy);

    //                fusedEnemy._vida += enemyToFuse._vida;
    //                _spatialGrid.Remove(enemyToFuse);
    //                EnemigoVoladorFactory.Instance.ReturnEnemy(enemyToFuse);
    //                enemiesInCell.Remove(enemyToFuse);
    //                GameManager.instance.arenaManager.enemigosEnLaArena.Remove(enemyToFuse);
    //                fusionCount++;
    //                Debug.Log("Fusion de enemigos");
    //            }

    //            if (enemiesInCell.Count > 0) 
    //            {
    //                var remainingFusedEnemy = enemiesInCell[0]; 
    //                _spatialGrid.Remove(remainingFusedEnemy);
    //                _spatialGrid.Add(remainingFusedEnemy);
    //                Debug.Log("Fusion de enemigos 2");
    //            }
    //        }
    //    }
    //}

}
