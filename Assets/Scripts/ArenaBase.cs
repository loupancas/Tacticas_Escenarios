using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArenaBase : MonoBehaviour
{
    public EnemigoBase[] enemigos;
    public SpawnPoints[] spawnPoints;
    public List<Nodo> nodos = new List<Nodo>();
    public List<EnemigoBase> enemigosEnLaArena;
    [SerializeField]protected bool _arenaEmpezada;


    public abstract void IniciarHorda();
}
