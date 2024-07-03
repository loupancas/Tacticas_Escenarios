using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    public static ProjectileFactory Instance { get { return _instance; } }
    static ProjectileFactory _instance;

    public Projectile projectilePrefab;
    public int stock = 10;

    public Pool<Projectile> pool;

    void Start()
    {
        _instance = this;
        pool = new Pool<Projectile>(ProjectileCreator, Projectile.TurnOnOff, stock);
    }

    public Projectile ProjectileCreator()
    {
        return Instantiate(projectilePrefab, transform);
    }

    public void ReturnProjectile(Projectile p)
    {
        pool.ReturnObject(p);
    }
}
