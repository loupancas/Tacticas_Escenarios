
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Base Weapon Components")]
    [SerializeField] protected Transform _shotTransform;
    [SerializeField] public Animator _weaponAnimator;
    [SerializeField] protected Renderer _weaponRenderer;
    [SerializeField] protected VisualEffect _particula;


    [Header("Base Weapon Stats")]
    [SerializeField] public float shotCooldown = .1f;
    [SerializeField] public int dmg = 5;
    [SerializeField] protected ShootType _gunType;

    [SerializeField] protected int _modifiedDmg;
    [SerializeField] protected float _modifiedCooldown;
    public ShootType gunType { get { return _gunType; } }

    public bool CanShoot { get { return _canShoot; } }
    protected bool _canShoot;

    protected Ray _ray;
    protected RaycastHit _rayHit;
    [SerializeField]protected LayerMask _shootableLayers;
    

    [SerializeField]protected AudioSource _audioSource;

    [SerializeField] protected AudioClip _shotSound;

    CountdownTimer _shotCooldownTimer;
    
    public void SetInitialParams(Transform shotTransform, LayerMask shootableLayers, int Fase)
    {
        _shotTransform = shotTransform;
        _shootableLayers = shootableLayers;

        Fases(Fase);

        _canShoot = true;
        _shotCooldownTimer = new CountdownTimer(_modifiedCooldown);
        _shotCooldownTimer.OnTimerStop = crFireCooldown;
        _shotCooldownTimer.OnTimerStart = crFireCooldown;
        _weaponAnimator.SetBool("Idle", true);
    }
    abstract protected void FireBehaviour();

    public virtual void Update()
    {
        _shotCooldownTimer.Tick(Time.deltaTime);
    }

    virtual public void Fire()
    {
        FireBehaviour();
        _shotCooldownTimer.Start();    
        _audioSource.clip = _shotSound;
        _audioSource.Play();
        _particula.Play();
        _weaponAnimator.SetTrigger("Shoot");

    }
    private void crFireCooldown()
    {
        _canShoot = !_canShoot;
    }

    public enum ShootType
    {
        Automatic,
        Burst,
        SemiAutomatic,
    }

    public void UpdateFase(int fase)
    {
        Fases(fase);
    }

    public abstract void Fases(int fase);
    
}
