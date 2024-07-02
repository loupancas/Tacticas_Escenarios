using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuego : MonoBehaviour
{
    [SerializeField] float _CooldownDamage;
    [SerializeField] int _dmg;
    CountdownTimer _timer;

    public void Start()
    {
        _timer = new CountdownTimer(_CooldownDamage);
        

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<FirstPersonPlayer>() != null)
        {
            other.GetComponent<FirstPersonPlayer>().TakeDamage(_dmg);
            _timer.Start();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<FirstPersonPlayer>() != null)
        {
            
            _timer.Tick(Time.deltaTime);
            
            
            if (_timer.IsFinished)
            {
                _timer.Reset();
                _timer.Start();
                other.GetComponent<FirstPersonPlayer>().TakeDamage(_dmg);
                
            }
            
        }
    }
}
