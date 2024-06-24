
using UnityEngine;

public class ActivadorDeSpawner : MonoBehaviour
{
    [SerializeField] ArenaBase _arena;
    [SerializeField] GameManager _gameManager;
    private void OnTriggerEnter(Collider other)
    {
        _gameManager.updateList = true;
        _arena.IniciarHorda();
        gameObject.SetActive(false);
    }
}
