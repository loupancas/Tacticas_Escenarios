using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public abstract class Entity : MonoBehaviour
{
    [Header("Values of entity")]
    [SerializeField] protected int _vidaMax;
    [SerializeField] protected int _vida;
    [SerializeField] public VisualEffect _damageParticle;
    public virtual void TakeDamage(int Damage)
    {
        _vida -= Damage;
        _damageParticle.Play();

        if (_vida < 0)
        {
            Morir();
        }
    }

    public abstract void Morir();




}
