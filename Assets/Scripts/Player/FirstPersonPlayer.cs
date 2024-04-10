using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonPlayer : Entity
{
    public static FirstPersonPlayer instance;

    public delegate void SaWardo();
    public SaWardo theWorld;

    [Header("Values")]
    public float movementSpeed = 5f;
    [Range(1f, 500f)][SerializeField] private float _mouseSensitivity = 100f;
    [SerializeField] private float _cooldownMeleeAttack;
    [SerializeField] private float _cooldownFreeze;
    [SerializeField] private float _dashCd;
    [SerializeField] private float _dashForce;
    [SerializeField] private float _dashUpwardForce;
    [SerializeField] private float _dashTime;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private int _maxJumpsCount;
    [SerializeField] private int _maxDashsCount;
    [Header("States")]
    public bool dashing;
    public bool grounded = true;
    [Header("Components")]
    [SerializeField] private Transform _head;
    [SerializeField] private AttackMelee _attackMelee;
    [Header("Controls")]
    [SerializeField] private KeyCode _fireKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _meleeKey = KeyCode.F;
    [SerializeField] private KeyCode _stopTime = KeyCode.E;
    [SerializeField] private KeyCode _dashKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [Header("Weapons")]
    [SerializeField] private List<WeaponBase> _weaponStash = new List<WeaponBase>();
    [SerializeField] private LayerMask _shootableLayers;
    


    private Rigidbody _rb;
    private BoxCollider _boxCol;
    private FirstPersonCamera _cam;
    private float _xAxis, _zAxis, _inputMouseX, _inputMouseY, _mouseX;
    private WeaponBase _equippedWeapon;

    player_Movement _movement;
    player_Inputs _inputs;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        _vida = _vidaMax;
 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _rb = GetComponent<Rigidbody>();
        _boxCol = GetComponent<BoxCollider>();

        _cam = Camera.main.GetComponent<FirstPersonCamera>();
        _cam.SetHead(_head);

        int numeroRandom = Random.Range(0, _weaponStash.Capacity);
        _equippedWeapon = _weaponStash[numeroRandom];
        _equippedWeapon.gameObject.SetActive(true);
        _equippedWeapon.SetInitialParams(_cam.transform, _shootableLayers);

        _movement = new player_Movement(this, movementSpeed, _mouseSensitivity, _mouseX, _cam, _rb, _dashForce, _dashUpwardForce, _jumpHeight, _maxJumpsCount, _boxCol);
        _inputs = new player_Inputs(_fireKey, _xAxis, _zAxis, _inputMouseX, _inputMouseY, _movement, _equippedWeapon, _attackMelee, _meleeKey, _stopTime, _cooldownFreeze, _dashKey, _jumpKey, this, _maxDashsCount);
        
    }

    public void Update()
    {
        _inputs.Rotation();
        _inputs.MeleeAttack();
        _inputs.TimeStop();
        _inputs.Jump();
        _inputs.Dash();
        _movement.GroundedState();
    }

    public void CambioDeArma()
    {
        if (_equippedWeapon != null)
        {
            _equippedWeapon.gameObject.SetActive(false);
            _equippedWeapon = null;
        }
        int numeroRandom = Random.Range(0, _weaponStash.Capacity);
        _equippedWeapon = _weaponStash[numeroRandom];
        _inputs.UpdateWeapon(_equippedWeapon);
        _equippedWeapon.gameObject.SetActive(true);
        _equippedWeapon.SetInitialParams(_cam.transform, _shootableLayers);

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

   
}
