using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerDeBalas : MonoBehaviour, IFreezed
{
    [SerializeField] ProyectilesBase _proyectil;
    [SerializeField] float _timeSpawn;
    public delegate void DelegateUpdate();
    public DelegateUpdate delegateUpdate;
    float _currTime = 0;
    CountdownTimer _Freezetime;


    void Start()
    {
        delegateUpdate = NormalUpdate;
        GameManager.instance.pj.theWorld += StoppedTime;
        _Freezetime = new CountdownTimer(3);
        _Freezetime.OnTimerStop = BackToNormal;
    }

    
    void Update()
    {
        delegateUpdate.Invoke();
    }
    public void Freezed()
    {
        _Freezetime.Tick(Time.deltaTime);
    }

    public void NormalUpdate()
    {
        _currTime += Time.deltaTime;
        if (_currTime > _timeSpawn)
        {
            _proyectil.SpawnProyectile(transform);
            _currTime = 0;
        }
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
}
