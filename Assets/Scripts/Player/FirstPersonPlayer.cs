using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonPlayer : Entity
{
    public static FirstPersonPlayer instance;
    

    [Header("Values")]
    public float movementSpeed = 5f;
    [Range(1f, 500f)][SerializeField] private float _mouseSensitivity = 100f;
    public bool lightIsActive;
    public bool bocinaIsActive;
    
    [Header("Components")]
    [SerializeField] private Transform _head;
    [SerializeField] private Light _light;
    [SerializeField] private Renderer _Hplow;
    [Header("Controls")]
    [SerializeField] private KeyCode _fireKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode _reloadKey = KeyCode.R;
    
    [Header("Weapons")]
    [SerializeField] private List<WeaponBase> _weaponStash = new List<WeaponBase>();
    [SerializeField] private LayerMask _shootableLayers;
    


    private Rigidbody _rb;
    private FirstPersonCamera _cam;
    private float _xAxis, _zAxis, _inputMouseX, _inputMouseY, _mouseX;
    private WeaponBase _equippedWeapon;
    public AudioSource audioSource;

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

        _cam = Camera.main.GetComponent<FirstPersonCamera>();
        _cam.SetHead(_head);

        _equippedWeapon = _weaponStash[0];
        _equippedWeapon.SetInitialParams(_cam.transform, _shootableLayers);
        
    }

    private void Update()
    {
        
        _xAxis = Input.GetAxisRaw("Horizontal");
        _zAxis = Input.GetAxisRaw("Vertical");


        _inputMouseX = Input.GetAxisRaw("Mouse X");
        _inputMouseY = Input.GetAxisRaw("Mouse Y");

       

        if(_inputMouseX != 0 || _inputMouseY != 0)
        {
            Rotation(_inputMouseX, _inputMouseY);
        }
        
        if (Input.GetKey(_fireKey) && _equippedWeapon.CanShoot)
        {
            _equippedWeapon.Fire();
        }
        else if(Input.GetKeyDown(_reloadKey) && !_equippedWeapon.IsReloading)
        {
            _equippedWeapon.Reload();
        }
    }

    public void TakeDamage(int damage)
    {
        _vida -= damage;
        _hudVida.ContadorVida(_vida);

        if(_vida < 0)
        {
            Morir();
        }
    }

    private void FixedUpdate()
    {
        if(_xAxis != 0 || _zAxis != 0)
        {
            Movement(_xAxis, _zAxis);
        }
    }

    private void Movement(float xAxis, float zAxis)
    {
        Vector3 dir = (transform.right * xAxis + transform.forward * zAxis).normalized;

        _rb.MovePosition(transform.position += dir * movementSpeed * Time.fixedDeltaTime);
    }

    private void Rotation(float xAxis, float yAxis)
    {
        _mouseX += xAxis * _mouseSensitivity * Time.deltaTime;

        if(_mouseX >= 360 || _mouseX <= -360)
        {
            _mouseX -= 360 * Mathf.Sign(_mouseX);
        }

        yAxis *= _mouseSensitivity * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0, _mouseX, 0);
        _cam?.Rotate(_mouseX, yAxis);
    }

    public void TakeAmmo(int Ammo)
    {
        _equippedWeapon.TakeAmmo(Ammo);
    }
    public override void Morir()
    {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        SceneManager.LoadScene(2);
    }

   
}
