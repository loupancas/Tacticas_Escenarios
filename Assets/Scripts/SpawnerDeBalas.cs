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

   

    void Start()
    {
        delegateUpdate = NormalUpdate;
        GameManager.instance.pj.theWorld += StoppedTime;
    }

    
    void Update()
    {
        delegateUpdate.Invoke();
    }
    public void Freezed()
    {

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
        StartCoroutine(StopTime());
    }

    public IEnumerator StopTime()
    {
        delegateUpdate = Freezed;
        yield return new WaitForSeconds(3);
        delegateUpdate = NormalUpdate;
    }

}
