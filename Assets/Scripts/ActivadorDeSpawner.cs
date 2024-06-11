using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivadorDeSpawner : MonoBehaviour
{
    [SerializeField] ArenaBase _arena;
    [SerializeField] GameManager gameManager;
    
    private void OnTriggerEnter(Collider other)
    {
        gameManager.updateList = true;
        _arena.IniciarHorda();
        gameObject.SetActive(false);
    }
}
