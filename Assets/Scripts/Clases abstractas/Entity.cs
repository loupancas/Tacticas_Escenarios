using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("Values of entity")]
    [SerializeField] protected int _vidaMax;
    protected int _vida;
    public void CurarVida(int Curacion)
    {
        _vida += Curacion;
        if (_vida > _vidaMax)
        {
            _vida = _vidaMax;
        }
    }

    public abstract void Morir();




}
