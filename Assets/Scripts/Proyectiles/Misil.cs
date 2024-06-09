using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misil : ProyectilesBase, IFreezed
{
    public delegate void DelegateUpdate();
    public DelegateUpdate delegateUpdate;
    CountdownTimer _Freezetime;
    bool devuelto;
    void Start()
    {
        delegateUpdate = NormalUpdate;
        GameManager.instance.pj.theWorld += StoppedTime;
        _Freezetime = new CountdownTimer(3);
        _Freezetime.OnTimerStop = BackToNormal;
        _maxDistance = 10;
    }
    public override void SpawnProyectile(Transform spawnPoint)
    {
        var p = MisilFactory.Instance.pool.GetObject();
        p.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.rotation.normalized);
    }

    private void Reset()
    {
        _currentDistance = 0;
        _modifiedDmg = _dmg;
        _modifiedSpeed = _speed;
        devuelto = false;
        delegateUpdate = NormalUpdate;
    }

    public static void TurnOnOff(Misil p, bool active = true)
    {
        if (active)
        {
            p.Reset();
        }
        p.gameObject.SetActive(active);
    }
    public void DevolverBala()
    {
        transform.forward = GameManager.instance.pj.cam.transform.forward;
        transform.rotation = GameManager.instance.pj.cam.transform.rotation;
        _modifiedSpeed = _speed * 2;
        _modifiedDmg = _dmg * 2;
    }

    // Update is called once per frame
    void Update()
    {
        delegateUpdate.Invoke();
    }
    public void NormalUpdate()
    {
        var distanceToTravel = _modifiedSpeed * Time.deltaTime;

        transform.position += transform.forward * distanceToTravel;

        _currentDistance += distanceToTravel;
        if (_currentDistance > _maxDistance)
        {
            MisilFactory.Instance.ReturnProjectile(this);
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<EnemigoBase>() != null)
        {
            print("Toco al enemigo");
            collision.collider.GetComponent<EnemigoBase>().TakeDamage(_dmg);
            collision.collider.GetComponent<EnemigoVolador>()?.AddDamage(_dmg);
            GameManager.instance.pj.AgregarBuff();

            MisilFactory.Instance.ReturnProjectile(this);
        }

        if(collision.collider.GetComponent<PuntosDebiles>() != null)
        {
            collision.collider.GetComponent<PuntosDebiles>().OnHit(_dmg);
            MisilFactory.Instance.ReturnProjectile(this);
        }
    }
}
