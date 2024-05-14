using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorDeSpawner : MonoBehaviour
{
    [SerializeField] ArenaManager _arena;

    private void OnTriggerEnter(Collider other)
    {
        _arena.IniciarHorda();
    }
}
