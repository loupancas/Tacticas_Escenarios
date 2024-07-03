using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMelee : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int _dmg;
    [SerializeField] float _lifeTime;
    CountdownTimer _timer;

    public void Start()
    {
        _timer = new CountdownTimer(_lifeTime);
        _timer.OnTimerStop = DesactivarAtaque;
        _timer.Start();
    }

    private void Update()
    {
        _timer.Tick(Time.deltaTime);    

    }

    public void MeleeAttack()
    {
        _timer.Start();
        gameObject.SetActive(true);
    }

    public void DesactivarAtaque()
    {
        gameObject.SetActive(false);
        _timer.Reset();
    }

    private void OnTriggerEnter(Collider other)
    {
         other.gameObject.GetComponent<EnemigoVolador>()?.TakeDamage(_dmg);
    }

}
