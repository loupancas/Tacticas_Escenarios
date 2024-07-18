
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
    [SerializeField] public VisualEffect _particula;


    [Header("Base Weapon Stats")]
    [SerializeField] public float shotCooldown = .1f;
    [SerializeField] public int dmg = 5;
    [SerializeField] protected ShootType _gunType;
    [SerializeField] protected float cameraRecoilAmount = 0.5f;
    [SerializeField] protected int _modifiedDmg;
    [SerializeField] protected float _modifiedCooldown;

    public ShootType gunType { get { return _gunType; } }

    public bool CanShoot { get { return _canShoot; } }
    protected bool _canShoot;

    protected Ray _ray;
    protected RaycastHit _rayHit;
    [SerializeField]protected LayerMask _shootableLayers;
    protected Transform cameraTransform;
    [SerializeField] private float cameraRecoilRecoverySpeed = 0.2f;
    private Vector3 cameraOriginalRotation;
    [SerializeField]protected AudioSource _audioSource;

    [SerializeField] protected AudioClip _shotSound;

    protected CountdownTimer _shotCooldownTimer;
    protected CountdownTimer _recoilRecoveryTimer;
    

    public void SetInitialParams(Transform shotTransform, LayerMask shootableLayers, int Fase)
    {
        _shotTransform = shotTransform;
        _shootableLayers = shootableLayers;
        cameraTransform = shotTransform;
        Fases(Fase);
        _shotCooldownTimer = new CountdownTimer(_modifiedCooldown);
        _shotCooldownTimer.OnTimerStop = crFireCooldown;
        _shotCooldownTimer.OnTimerStart = crFireCooldown;
        _canShoot = true;
        _recoilRecoveryTimer = new CountdownTimer(cameraRecoilRecoverySpeed);
        _recoilRecoveryTimer.OnTimerStop = _recoilRecoveryTimer.Reset;
        _weaponAnimator.SetBool("Idle", true);
        _particula.enabled = false;



    }
    abstract protected void FireBehaviour();

    public virtual void Update()
    {
        var x = Input.GetAxis("Mouse X");
        var y = Input.GetAxis("Mouse Y");

        print(x);

        _shotCooldownTimer.Tick(Time.deltaTime);
        _recoilRecoveryTimer.Tick(Time.deltaTime);

        //if (_recoilRecoveryTimer.IsRunning && x == 0 && y == 0)
        //{
           
        //    cameraTransform.localEulerAngles = Vector3.Lerp(cameraTransform.localEulerAngles, cameraOriginalRotation, Time.deltaTime * cameraRecoilRecoverySpeed);
        //}

    }

    virtual public void Fire()
    {
        Debug.Log(_particula);
        _particula.enabled = true;
        _particula.Play();
        FireBehaviour();
        _shotCooldownTimer.Start();    
        _audioSource.clip = _shotSound;
        _audioSource.Play();
        _particula.enabled=true;

        

        cameraOriginalRotation = cameraTransform.localEulerAngles;
        _recoilRecoveryTimer.Start();
        cameraTransform.localEulerAngles += new Vector3(-cameraRecoilAmount, 0, 0);
        //cameraTransform.localEulerAngles = Vector3.Lerp(cameraTransform.localEulerAngles, cameraOriginalRotation, Time.deltaTime * cameraRecoilRecoverySpeed);
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
        print("Update fase : WeaponBase");
        Fases(fase);
        //print(_modifiedCooldown);
        _shotCooldownTimer.Reset(_modifiedCooldown);
        //bool existo = _shotCooldownTimer != null;
        //print("El timer del cooldown existe: " + existo);
    }

    public abstract void Fases(int fase);
    
}
