using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : ProyectilesBase, IFreezed
{
    public delegate void DelegateUpdate();
    public DelegateUpdate delegateUpdate;
    void Start()
    {
        delegateUpdate = NormalUpdate;
        GameManager.instance.pj.theWorld += StoppedTime;
        //_dmg = ProyectilesStats.ProyectilNormal.dmg;
        //_speed = ProyectilesStats.ProyectilNormal.speed;
        //_maxDistance = ProyectilesStats.ProyectilNormal.maxDistance;
    }

    
    void Update()
    {
        delegateUpdate.Invoke();

        /*var distanceToTravel = _speed * Time.deltaTime;

        transform.position += transform.forward * distanceToTravel;

        _currentDistance += distanceToTravel;
        if (_currentDistance > _maxDistance)
        {
            ProjectileFactory.Instance.ReturnProjectile(this);
        }
        */
    }

    private void Reset()
    {
        _currentDistance = 0;
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
        if (collision.collider.GetComponent<Entity>() != null)
        {
            collision.collider.GetComponent<Entity>().TakeDamage(_dmg);
            ProjectileFactory.Instance.ReturnProjectile(this);
        }   
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<AttackMelee>() != null)
        {
            print("Toco el trigger");
            ProjectileFactory.Instance.ReturnProjectile(this);

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
        var distanceToTravel = _speed * Time.deltaTime;

        transform.position += transform.forward * distanceToTravel;

        _currentDistance += distanceToTravel;
        if (_currentDistance > _maxDistance)
        {
            ProjectileFactory.Instance.ReturnProjectile(this);
        }
    }

    public void Freezed()
    {
        
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
