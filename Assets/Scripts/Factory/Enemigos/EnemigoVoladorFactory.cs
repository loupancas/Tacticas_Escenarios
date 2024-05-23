using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoVoladorFactory : MonoBehaviour
{
    public static EnemigoVoladorFactory Instance { get { return _instance; } }
    static EnemigoVoladorFactory _instance;

    public EnemigoVolador projectilePrefab;
    public int stock = 10;

    public Pool<EnemigoVolador> pool;

    void Start()
    {
        _instance = this;
        pool = new Pool<EnemigoVolador>(ProjectileCreator, EnemigoVolador.TurnOnOff, stock);
    }

    public EnemigoVolador ProjectileCreator()
    {
        return Instantiate(projectilePrefab, transform);
    }

    public void ReturnProjectile(EnemigoVolador p)
    {
        pool.ReturnObject(p);
    }
}
