using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisilFactory : MonoBehaviour
{
    public static MisilFactory Instance { get { return _instance; } }
    static MisilFactory _instance;

    public Misil projectilePrefab;
    public int stock = 10;

    public Pool<Misil> pool;

    void Start()
    {
        _instance = this;
        pool = new Pool<Misil>(ProjectileCreator, Misil.TurnOnOff, stock);
    }

    public Misil ProjectileCreator()
    {
        return Instantiate(projectilePrefab, transform);
    }

    public void ReturnProjectile(Misil p)
    {
        pool.ReturnObject(p);
    }
}
