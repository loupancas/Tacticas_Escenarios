using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : ProyectilesBase, IFreezed
{
    public bool devuelto = false;
    public delegate void DelegateUpdate();
    public DelegateUpdate delegateUpdate;

    CountdownTimer _Freezetime;
    void Start()
    {
        delegateUpdate = NormalUpdate;
        _Freezetime = new CountdownTimer(10);
        _Freezetime.OnTimerStop = BackToNormal;
        GameManager.instance.pj.theWorld += StoppedTime;
        _modifiedDmg = _dmg;
        _modifiedSpeed = _speed;
    }

    
    void Update()
    {
        delegateUpdate.Invoke();

        
    }

    private void Reset()
    {
        _currentDistance = 0;
        _modifiedDmg = _dmg;
        _modifiedSpeed = _speed;
        devuelto = false;
        delegateUpdate = NormalUpdate;
    }

    public static void TurnOnOff(Projectile p, bool active = true)
    {
        if (active)
        {
            p.Reset();
        }
        p.gameObject.SetActive(active);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<FirstPersonPlayer>() != null)
        {
            collision.collider.GetComponent<FirstPersonPlayer>().TakeDamage(_modifiedDmg);
            ProjectileFactory.Instance.ReturnProjectile(this);
        }   
        
        if (collision.collider.GetComponent<EnemigoBase>() != null && devuelto)
        {
            print("Toco al enemigo");
            collision.collider.GetComponent<EnemigoBase>().Morir();
            GameManager.instance.pj.AgregarBuff();
            
            ProjectileFactory.Instance.ReturnProjectile(this);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<AttackMelee>() != null)
        {
            print("Toco el trigger");

            DevolverBala();
        }
    }
    public override void SpawnProyectile(Transform spawnPoint)
    {
        var p = ProjectileFactory.Instance.pool.GetObject();
        p.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.rotation.normalized);
        Debug.Log("Disparo proyectil");
    }

    public void NormalUpdate()
    {
        var distanceToTravel = _modifiedSpeed * Time.deltaTime;

        transform.position += transform.forward * distanceToTravel;

        _currentDistance += distanceToTravel;
        if (_currentDistance > _maxDistance)
        {
            ProjectileFactory.Instance.ReturnProjectile(this);
        }
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
    public void DevolverBala()
    {
        devuelto = true;
        transform.forward = GameManager.instance.pj.cam.transform.forward;
        transform.rotation = GameManager.instance.pj.cam.transform.rotation;
        _modifiedDmg = _dmg * 2;
        _modifiedSpeed = _speed * 6;
    }

    
}
