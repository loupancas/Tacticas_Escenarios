using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntosDebiles : MonoBehaviour
{
    [SerializeField] EnemigoVolador _me;
    [SerializeField] MeshRenderer _mesh;
    public bool IsActive;
    public int resistance;
    public GameObject _puntoDebil;
    public void Awake()
    {
        int random = Random.Range(1, 10);
        resistance = random;

    }
    public void Activate()
    {
        _mesh.material.color = Color.red;
        _puntoDebil.SetActive(true);
        _puntoDebil.GetComponent<MeshRenderer>().enabled = false;
    }
    public void OnHit(int Dmg)
    {
        //if(IsActive)
        //{
        //    _me.Morir();
        //    GameManager.instance.pj.AgregarBuff();
            
        //}
        //else
        //{
            _me.TakeDamage(resistance + Dmg);
        //}
        
    }
    public void Desactivate()
    {
        _mesh.material.color = Color.grey;
    }
}
