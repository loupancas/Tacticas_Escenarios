using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : Entity
{
    [SerializeField]PuntosDebiles[] _puntosDebiles;

    public void Awake()
    {
        
    }
    public void Start()
    {
        _vida = _vidaMax;
        int NumeroRandom = Random.Range(0, _puntosDebiles.Length);
        print(NumeroRandom);
        _puntosDebiles[NumeroRandom].IsActive = true;
        _puntosDebiles[NumeroRandom].Activate();

    }
    public override void Morir()
    {
        print("Mori xd");
        FirstPersonPlayer.instance.CambioDeArma();
    }

    
}
