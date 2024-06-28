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
    [SerializeField] private TextoActualizable _textDashCounts, _textJumpCounts, _textFases;
    [Header("Controls")]
    [SerializeField] Controles _control; 
    [Header("Weapons")]
    [SerializeField] private List<WeaponBase> _weaponStash = new List<WeaponBase>();
    [SerializeField] private LayerMask _shootableLayers;
    


    private Rigidbody _rb;
    public FirstPersonCamera cam;
    public WeaponBase equippedWeapon;

    player_Movement _movement;
    player_Inputs _inputs;
    ModifierStat _buffs;
    Player_Fases _fases;
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
        _buffs.ArmaUpdate(equippedWeapon);
        _buffs.UpdateBuffs();
        _vida = _vidaMax;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _rb = GetComponent<Rigidbody>();
        

        cam = Camera.main.GetComponent<FirstPersonCamera>();
        cam.SetHead(_head);

        _fases = new Player_Fases(5, _textFases);

        int numeroRandom = UnityEngine.Random.Range(0, _weaponStash.Capacity);
        equippedWeapon = _weaponStash[numeroRandom];
        equippedWeapon.gameObject.SetActive(true);
        equippedWeapon.SetInitialParams(cam.transform, _shootableLayers, _fases.fases);


        _movement = new player_Movement(this, _textJumpCounts, _buffs, _control);
        _inputs = new player_Inputs( _movement, equippedWeapon, this, _textDashCounts, _buffs, _control, _attackMelee, _textJumpCounts);

    }

    public void Update()
    {
        _inputs.ControlUpdateNormal();
        _buffs.Update();
        _inputs.Rotation();
        _inputs.MeleeAttack();
        _inputs.TimeStop();
        _fases.UpdateTimer(Time.deltaTime);
    }

    public void CambioDeArma()
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.GetComponent<Animator>().SetTrigger("Swap");

            equippedWeapon.gameObject.SetActive(false);
            equippedWeapon = null;
        }

        int numeroRandom = UnityEngine.Random.Range(0, _weaponStash.Capacity);
        equippedWeapon = _weaponStash[numeroRandom];
        _buffs.ArmaUpdate(equippedWeapon);
        _inputs.UpdateWeapon(equippedWeapon);

        equippedWeapon.gameObject.SetActive(true);
        equippedWeapon.SetInitialParams(cam.transform, _shootableLayers, _fases.fases);
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
        print("Buffo dado");
        _fases.SubirFase();
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
