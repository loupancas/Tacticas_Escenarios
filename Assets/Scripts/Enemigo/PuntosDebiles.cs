using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntosDebiles : MonoBehaviour
{
    [SerializeField] EnemigoVolador _me;
    [SerializeField] MeshRenderer _mesh;
    public bool IsActive;
    public void Start()
    {
        

    }
    public void Activate()
    {
        _mesh.material.color = Color.red;
    }
    public void OnHit(int Dmg)
    {
        if(IsActive)
        {
            _me.Morir();
        }
        else
        {
            _me.TakeDamage(Dmg);
        }
        
    }
}
