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
    CountdownTimer _timerToCharge, _timerOnCharge;
    [SerializeField] float _maxVelocity;
    [SerializeField] float _maxForce;
    Vector3 _velocity;
    bool _OnCharge = false;
    [SerializeField] float _distance;
    [SerializeField] float _timeToCharge, _timeOnCharge;
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



        _timerToCharge = new CountdownTimer(_timeToCharge);

        _timerToCharge.OnTimerStart = OnCharge;
        _timerToCharge.OnTimerStop = SetTarget;
        _timerToCharge.Start();

        
    }
    public override IState ProcessInput()
    {
        return this;
    }

    public override void UpdateLoop()
    {
        _timerToCharge.Tick(Time.deltaTime);
        
        if (_OnCharge)
        {
            AddForce(_target.transform.position);

            if(Vector3.Distance(transform.position, _target.transform.position) <= 0.5f || _timerOnCharge.IsFinished)
            {
                _rb.velocity = Vector3.zero;
                _OnCharge = false;
                _timerToCharge.Reset();
                _timerToCharge.Start();
            }
        }

        _me.transform.position += _velocity * Time.deltaTime;
        _me.transform.forward = _velocity;
    }
    
    

    public void OnCharge()
    {
        transform.LookAt(GameManager.instance.pj.transform);
        _target.transform.position = new Vector3(GameManager.instance.pj.transform.position.x * _distance, 1, GameManager.instance.pj.transform.position.z * _distance);
        
    }

    public void SetTarget()
    {
        print("Arranco");
        _OnCharge = true;
        
        //_timerOnCharge.Start();
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
