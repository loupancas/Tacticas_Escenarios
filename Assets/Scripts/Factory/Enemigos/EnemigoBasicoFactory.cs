using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoBasicoFactory : MonoBehaviour
{
    public static EnemigoBasicoFactory Instance { get { return _instance; } }
    static EnemigoBasicoFactory _instance;

    public EnemigoBasico flyerEnemyPrefab;
    public int stock = 10;

    public Pool<EnemigoBasico> pool;

    void Start()
    {
        _instance = this;
        pool = new Pool<EnemigoBasico>(ProjectileCreator, EnemigoBasico.TurnOnOff, stock);
    }

    public EnemigoBasico ProjectileCreator()
    {
        return Instantiate(flyerEnemyPrefab, transform);
    }

    public void ReturnEnemy(EnemigoBasico p)
    {
        pool.ReturnObject(p);
    }
}
