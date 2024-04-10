
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Base Weapon Components")]
    [SerializeField] protected Transform _shotTransform;
    [SerializeField] protected Animator _weaponAnimator;
    [SerializeField] protected Renderer _weaponRenderer;
    [SerializeField] protected ParticleSystem _particula;

    [Header("Base Weapon Stats")]
    [SerializeField] private float _shotCooldown = .1f;
    [SerializeField] private float _reloadCooldown = 3f;
    [SerializeField] protected int _dmg = 5;
    [SerializeField] public int _totalAmmoStash = 120;
    [SerializeField] private int _ammoPerMag = 30;
    [SerializeField] private int _actualMag = 30;
    
    public bool CanShoot { get { return _canShoot; } }
    protected bool _canShoot;
    public bool IsReloading { get { return _isReloading; } }
    protected bool _isReloading;

    protected Ray _ray;
    protected RaycastHit _rayHit;
    protected LayerMask _shootableLayers;
    
    [SerializeField]protected AudioSource _audioSource;

    [SerializeField]protected AudioClip _shotSound, _reloadSound;
    
    
    public void SetInitialParams(Transform shotTransform, LayerMask shootableLayers)
    {
        _shotTransform = shotTransform;
        _shootableLayers = shootableLayers;

        _canShoot = true;
    }

    abstract protected void FireBehaviour();

    virtual public void Fire()
    {
        
        
        if (_actualMag <= 0)
        {
            Reload();
        }
        else
        {
            _actualMag--;
            FireBehaviour();
            StartCoroutine(crFireCooldown());
            
            _audioSource.clip = _shotSound;
            _audioSource.Play();
        }
        _particula.Play();
    }

    virtual public void Reload()
    {
        if(_totalAmmoStash > 0 && _actualMag < _ammoPerMag)
        {
            int neededAmmo = _ammoPerMag - _actualMag;

            if(neededAmmo <= _totalAmmoStash)
            {
                _totalAmmoStash -= neededAmmo;
                _actualMag = _ammoPerMag;
            }
            else
            {
                _actualMag += _totalAmmoStash;
                _totalAmmoStash = 0;
            }

            StartCoroutine(crReloadCooldown());
        }
        else
        {
            if(_totalAmmoStash <= 0)
            {
                
            }
        }
    }

    private IEnumerator crFireCooldown()
    {
        _canShoot = !_canShoot;
        yield return new WaitForSeconds(_shotCooldown);
        _canShoot = !_canShoot;
    }

    private IEnumerator crReloadCooldown()
    {
        print($"RELOADING!");
        _isReloading = !_isReloading;
        _canShoot = !_canShoot;
        _weaponAnimator.SetBool("IsReloading", _isReloading);
        yield return new WaitForSeconds(_reloadCooldown);
        _isReloading = !_isReloading;
        _canShoot = !_canShoot;
        _weaponAnimator.SetBool("IsReloading", _isReloading);
        print($"Fresh mag loaded.");
        _weaponRenderer.material.SetFloat("currBullet", _actualMag);
    }
    public void TakeAmmo(int Ammo)
    {
       _totalAmmoStash += Ammo;
    }

    
}
