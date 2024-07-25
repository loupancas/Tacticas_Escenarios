using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using UnityEngine.AI;

public class EnemigoBasicoAtaqueMelee : MonoBaseState
{
    EnemigoBasico _me;
    CountdownTimer _timerToAttack;
    [SerializeField] Transform _hitTransform;
    [SerializeField] float _cooldownAttack;
    [SerializeField] float _distanceToAttack;
    [SerializeField] int _AttackDamage;
    protected RaycastHit _rayHit;
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        _me = gameObject.GetComponent<EnemigoBasico>();

        _timerToAttack = new CountdownTimer(_cooldownAttack);

        _timerToAttack.OnTimerStop = Attack;
        Attack();
    }

    public override IState ProcessInput()
    {
        if (_me.IsHPLowThat(20) && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        if (!_me.IsAttackDistanceMelee() && Transitions.ContainsKey(StateTransitions.ToPersuit))
            return Transitions[StateTransitions.ToPersuit];

        return this;
    }

    public override void UpdateLoop()
    {
        _timerToAttack.Tick(Time.deltaTime);
        
    }

    public void Attack()
    {
        
        Ray ray = new Ray(_hitTransform.position, _hitTransform.forward);
        //_ray = new Ray(_shotTransform.position, _shotTransform.forward);
        Debug.DrawRay(ray.origin, ray.direction * _distanceToAttack, Color.red, 2.0f);
        if (Physics.Raycast(ray, out _rayHit, _distanceToAttack))
        {

            if (_rayHit.collider.GetComponent<Entity>())
            {
                _rayHit.collider.GetComponent<Entity>().TakeDamage(_AttackDamage);
                Debug.Log("Choque y te hice daño");

            }
        }

        _timerToAttack.Reset();
        _timerToAttack.Start();
    }

    private void OnDrawGizmos()
    {
        
    }
}
