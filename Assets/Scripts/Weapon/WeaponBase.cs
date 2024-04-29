
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
    [SerializeField] public float shotCooldown = .1f;
    [SerializeField] public int dmg = 5;
    [SerializeField] protected ShootType _gunType;
    public ShootType gunType { get { return _gunType; } }

    public bool CanShoot { get { return _canShoot; } }
    protected bool _canShoot;

    protected Ray _ray;
    protected RaycastHit _rayHit;
    [SerializeField]protected LayerMask _shootableLayers;
    

    [SerializeField]protected AudioSource _audioSource;

    [SerializeField] protected AudioClip _shotSound;
    
    
    public void SetInitialParams(Transform shotTransform, LayerMask shootableLayers)
    {
        _shotTransform = shotTransform;
        _shootableLayers = shootableLayers;
        
        _canShoot = true;
    }
    abstract protected void FireBehaviour();
    virtual public void Fire()
    {
        FireBehaviour();
        StartCoroutine(crFireCooldown());   
        _audioSource.clip = _shotSound;
        _audioSource.Play();
        //_particula.Play();
    }
    private IEnumerator crFireCooldown()
    {
        _canShoot = !_canShoot;
        yield return new WaitForSeconds(shotCooldown);
        _canShoot = !_canShoot;
    }

    public enum ShootType
    {
        Automatic,
        Burst,
        SemiAutomatic,
    }
    
    
}
