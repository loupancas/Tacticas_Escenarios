
using UnityEngine;

public class ActivadorDeSpawner : MonoBehaviour
{
    [SerializeField] ArenaBase _arena;
<<<<<<< Updated upstream
    [SerializeField] GameManager _gameManager;
    private void OnTriggerEnter(Collider other)
    {
        _gameManager.updateList = true;
=======

    private void OnTriggerEnter(Collider other)
    {
>>>>>>> Stashed changes
        _arena.IniciarHorda();
        gameObject.SetActive(false);
    }
}
