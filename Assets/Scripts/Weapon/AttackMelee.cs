using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMelee : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int _dmg;
    [SerializeField] float _lifeTime;

    public IEnumerator SpawnTime()
    {
        print("Funciono");
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<EnemigoVolador>()?.TakeDamage(_dmg);

        
        
    }

}
