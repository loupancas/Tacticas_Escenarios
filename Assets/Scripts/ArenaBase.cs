using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArenaBase : MonoBehaviour
{
    public EnemigoBase[] enemigos;
    public GameObject[] spawnPoints;
    public List<Nodo> nodos = new List<Nodo>();
    public List<EnemigoBase> enemigosEnLaArena;


    public abstract void IniciarHorda();
}
