using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorDeSpawner : MonoBehaviour
{
    [SerializeField] ArenaBase _arena;

    private void OnTriggerEnter(Collider other)
    {
        _arena.IniciarHorda();
        gameObject.SetActive(false);
    }
}
