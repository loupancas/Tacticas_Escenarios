using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using UnityEngine.AI;
public class AttackTorturado : MonoBaseState
{
    NavMeshAgent _agent;
    Torturado _me;
    [SerializeField]GameObject _target;
    CountdownTimer _timerOnCharge;
    [SerializeField] float _maxVelocity;
    [SerializeField] float _maxForce;
    Vector3 _velocity;
    [SerializeField]bool _OnCharge = false, _HitWall = false;
    [SerializeField] float _distance;
    [SerializeField] float _timeOnCharge;
    Rigidbody _rb;

    private void Start()
    {
        _me = gameObject.GetComponent<Torturado>();
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        _timerOnCharge = new CountdownTimer(_timeOnCharge);
        _timerOnCharge.Start();

        _OnCharge = true;
        _HitWall = false;
        


        transform.LookAt(_target.transform);

        
    }
    public override IState ProcessInput()
    {
        if (!_me.InLineOfSight(transform.position, GameManager.instance.pj.transform.position) && !_OnCharge && Transitions.ContainsKey(StateTransitions.ToSearch))
            return Transitions[StateTransitions.ToSearch];

        if (_me.InLineOfSight(transform.position, GameManager.instance.pj.transform.position) && !_OnCharge && Transitions.ContainsKey(StateTransitions.ToCharge))
            return Transitions[StateTransitions.ToCharge];

        

        return this;
    }

    public override void UpdateLoop()
    {
        _timerOnCharge.Tick(Time.deltaTime);
        
        AddForce(Seek(_target.transform.position));

        if(Vector3.Distance(transform.position, _target.transform.position) <= 0.5f || _timerOnCharge.IsFinished)
        {
            print("Termino la embestida");
            _rb.velocity = Vector3.zero;
            _OnCharge = false;
            
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f))
        {
            Gizmos.DrawLine(transform.position, hit.point);
            if (hit.collider.gameObject.layer == 8)
            {
                _OnCharge = false;
                _HitWall = true;
                print("Golpeo pared");
            }
        }
        

        _me.transform.position += _velocity * Time.deltaTime;
        _me.transform.forward = _velocity;
    }
    
    

    

    Vector3 Seek(Vector3 dir)
    {
        var desired = dir - _me.transform.position;
        desired.Normalize();
        desired *= _maxVelocity;

        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce);

        return steering;
    }
    void AddForce(Vector3 dir)
    {
        _velocity += dir;

        _velocity = Vector3.ClampMagnitude(_velocity, _maxVelocity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, _target.transform.position);

        
        //Gizmos.color = isPath ? Color.green : Color.red;

        //Gizmos.DrawSphere(transform.position, 0.5f);

        //Gizmos.color = Color.blue;

        //foreach (var node in neighbour)
        //{
        //    Gizmos.DrawLine(transform.position, node.transform.position);
        //}
    }
}
