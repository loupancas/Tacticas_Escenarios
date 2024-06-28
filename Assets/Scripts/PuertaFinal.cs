using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaFinal : Puerta
{
    CondicionDeVictoria _codigo;

    public void Start()
    {
        _codigo = gameObject.GetComponent<CondicionDeVictoria>();
    }
    public override void Desbloquear()
    {
        gameObject.SetActive(true);
        _codigo.IsActive = true;
    }
}
