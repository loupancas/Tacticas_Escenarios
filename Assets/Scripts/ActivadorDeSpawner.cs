
using UnityEngine;

public class ActivadorDeSpawner : MonoBehaviour
{
    [SerializeField] ArenaBase _arena;
    [SerializeField] GameManager _gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<FirstPersonPlayer>() != null)
        {
            _arena.IniciarHorda();
            gameObject.SetActive(false);
            _gameManager.updateList = true;
        }
        
    }
}
