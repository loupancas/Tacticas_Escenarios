using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour, IFreezed
{
    public int horda;

    public EnemigoVolador enemigo;
    
    public GameObject[] spawnPoints;

    public List<EnemigoVolador> enemigosEnLaArena;

    CountdownTimer _timer;

    [SerializeField] float _timeSpawn;

    public delegate void DelegateUpdate();
    public DelegateUpdate delegateUpdate;


    private void Start()
    {
        _timer = new CountdownTimer(_timeSpawn);
        
        _timer.Start();

        delegateUpdate = NormalUpdate;


    }
    public void UpdateArena()
    {
        if(enemigosEnLaArena.Count > 0)
        {
            horda++;
            _timer.Reset();
        }
    }

    private void Update()
    {
        delegateUpdate.Invoke();
    }
    
    public void SpawnEnemy()
    {
        int NumeroRandom = Random.Range(0, spawnPoints.Length);
        print(NumeroRandom);
        enemigo.SpawnEnemy(spawnPoints[NumeroRandom].transform);
    }

    public void StoppedTime()
    {
        
    }

    public void NormalUpdate()
    {
        _timer.Tick(Time.deltaTime);


        if (_timer.IsFinished && enemigosEnLaArena.Count <= 10)
        {
            SpawnEnemy();
            _timer.Reset();
            _timer.Start();
        }
    }

    public void Freezed()
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
