using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;


[RequireComponent(typeof(Rigidbody))]
public class FirstPersonPlayer : Entity
{
    public static FirstPersonPlayer instance;

    public delegate void SaWardo();
    public SaWardo theWorld;

    [Header("Values")]
    [SerializeField] BaseStatsPlayer baseStats;
    public Stats Stats { get; private set; }
    
    [Header("States")]
    public bool dashing;
    public bool grounded = true;
    [Header("Components")]
    [SerializeField] private Transform _head;
    [SerializeField] private AttackMelee _attackMelee;
    [SerializeField] private HealthBar _hpBar;
    [SerializeField] private TextoActualizable _textDashCounts, _textJumpCounts;
    [Header("Controls")]
    [SerializeField] Controles _control; 
    [Header("Weapons")]
    [SerializeField] private List<WeaponBase> _weaponStash = new List<WeaponBase>();
    [SerializeField] private LayerMask _shootableLayers;
    


    private Rigidbody _rb;
    public FirstPersonCamera cam;
    private WeaponBase _equippedWeapon;

    player_Movement _movement;
    player_Inputs _inputs;
    ModifierStat _buffs;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        
        
    }
    private void Start()
    {
        _buffs = new ModifierStat(baseStats.baseStats);
        _buffs.ArmaUpdate(_equippedWeapon);
        _buffs.UpdateBuffs();
        _vida = _vidaMax;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _rb = GetComponent<Rigidbody>();
        

        cam = Camera.main.GetComponent<FirstPersonCamera>();
        cam.SetHead(_head);

        int numeroRandom = UnityEngine.Random.Range(0, _weaponStash.Capacity);
        _equippedWeapon = _weaponStash[numeroRandom];
        _equippedWeapon.gameObject.SetActive(true);
        _equippedWeapon.SetInitialParams(cam.transform, _shootableLayers);


        _movement = new player_Movement(this, _textJumpCounts, _buffs, _control);
        _inputs = new player_Inputs( _movement, _equippedWeapon, this, _textDashCounts, _buffs, _control, _attackMelee, _textJumpCounts);

    }

    public void Update()
    {
        _inputs.ControlUpdateNormal();
        _buffs.Update();
        _inputs.Rotation();
        _inputs.MeleeAttack();
        _inputs.TimeStop();
        //_inputs.Jump();
        //_inputs.Dash();
        _movement.GroundedState();
        _movement.DashState();
    }

    public void CambioDeArma()
    {
        if (_equippedWeapon != null)
        {
            _equippedWeapon.gameObject.SetActive(false);
            _equippedWeapon = null;
        }
        int numeroRandom = UnityEngine.Random.Range(0, _weaponStash.Capacity);
        _equippedWeapon = _weaponStash[numeroRandom];
        _buffs.ArmaUpdate(_equippedWeapon);
        _inputs.UpdateWeapon(_equippedWeapon);
        
        _equippedWeapon.gameObject.SetActive(true);
        _equippedWeapon.SetInitialParams(cam.transform, _shootableLayers);

    }

    private void FixedUpdate()
    {
        _inputs.Control();
        
    }
    public override void Morir()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        SceneManager.LoadScene(2);
    }

    public override void TakeDamage(int Damage)
    {
        base.TakeDamage(Damage);
        _hpBar.UpdateHPBar(_vida);
    }

    public void AgregarBuff()
    {
        int Numero = UnityEngine.Random.Range(0, 5);

        switch(Numero)
        {
            case 0:
                _buffs.Add("Aumento de velocidad", (Stats original) => { original.movementSpeed += 2; return original; }, 5);
                print("Aumento de velocidad");
                break;
            case 1:
                _buffs.Add("Daño Aumentado", (Stats original) => { original.cooldownFreeze -= 1; return original; }, 5);
                print("Daño Aumentado");
                break;
            case 2:
                _buffs.Add("Altura de salto aumentados", (Stats original) => { original.jumpHeight += 2; return original; }, 5);
                print("Altura de salto aumentados");
                break;
            case 3:
                _buffs.Add("Cooldown parar tiempo", (Stats original) => { original.cooldownFreeze -= 2; return original; }, 5);
                print("Cooldown parar tiempo");
                break;
            case 4:
                _buffs.Add("Dash mejorado", (Stats original) => { original.dashForce += 2; original.dashUpwardForce += 2; return original; }, 5);
                print("Dash mejorado");
                break;
        }
    }
}
