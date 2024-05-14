using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour, IFreezed
{
    public int horda;

    public EnemigoBase[] enemigos;
    
    public GameObject[] spawnPoints;

    public List<EnemigoBase> enemigosEnLaArena;

    CountdownTimer _timer;

    [SerializeField] float _timeSpawn;

    public delegate void DelegateUpdate();
    public DelegateUpdate delegateUpdate;


    private void Start()
    {
        delegateUpdate = NormalUpdate;
        GameManager.instance.pj.theWorld += StoppedTime;
        _timer = new CountdownTimer(_timeSpawn);
        _timer.Start();
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
        int NumeroRandom1 = Random.Range(0, spawnPoints.Length);
        int NumeroRandom2 = Random.Range(0, enemigos.Length);
        enemigos[NumeroRandom2].SpawnEnemy(spawnPoints[NumeroRandom1].transform);
        
        
    }

    public void StoppedTime()
    {
        _timer.Stop();
    }

    public void NormalUpdate()
    {
        for (int i = 0; i <= enemigos.Length; i++)
        {
            SpawnEnemy();
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
