using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System.Linq;

public class SearchEnemigoVolador : MonoBaseState
{
    [SerializeField] EnemigoVolador _me;
    [SerializeField] LayerMask _enemies;
    Vector3 _velocity;
    [SerializeField] float _maxVelocity;
    [SerializeField] float _maxForce;
    [SerializeField] Node endNode = null;
    [SerializeField] Node startNode = null;
    AStar<Node> _aStar;
    [SerializeField]List<Node> _path;
    Rigidbody _rb;
    [SerializeField] bool _isPathNotFound;
    public override IState ProcessInput()
    {
        if (_me.InLineOfSight(transform.position, GameManager.instance.pj.transform.position) && Transitions.ContainsKey(StateTransitions.ToPersuit))
            return Transitions[StateTransitions.ToPersuit];

        if (_me.InLineOfSight(transform.position, GameManager.instance.pj.transform.position) && _me.IsAttackDistance() && Transitions.ContainsKey(StateTransitions.ToAttack))
            return Transitions[StateTransitions.ToAttack];

        if(_isPathNotFound && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];

        return this;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);

        _isPathNotFound = false;
        _aStar = new AStar<Node>();
        _aStar.OnPathCompleted += GetPath;
        _aStar.OnCantCalculate += PathNotFound;
        _rb = gameObject.GetComponent<Rigidbody>();
        startNode = GameManager.instance.arenaManager.GetMinNode(transform.position);
        endNode = GameManager.instance.arenaManager.GetMinNode(GameManager.instance.pj.transform.position);
        StartCoroutine(_aStar.Run(startNode, IsGoal, Explode, GetHeuristic));
    }

    private void Start()
    {
        
    }
    public override void UpdateLoop()
    {
        endNode = GameManager.instance.arenaManager.GetMinNode(GameManager.instance.pj.transform.position);

        if (_path.Count > 0)
        {
            var dir = _path[0].transform.position - transform.position;
            _rb.excludeLayers = _enemies;
            AddForce(Seek(_path[0].transform.position));

            if (dir.magnitude <= 0.5f)
            {
                _path.RemoveAt(0);
                _rb.velocity = Vector3.zero;
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

    void GetPath(IEnumerable<Node> path)
    {

        foreach (Node node in path)
        {
            Debug.Log(node.name);

            _path = path.ToList();

            node.isPath = true;
        }
    }

    private bool IsGoal(Node node)
    {
        return node == endNode;
    }
    private float GetHeuristic(Node node)
    {
        return node.heuristic;
    }
    private IEnumerable<WeightedNode<Node>> Explode(Node node)
    {
        return node.neighbour.Select(neighbour => new WeightedNode<Node>(neighbour, 1f));
    }

    void PathNotFound()
    {
        Debug.Log("Path not found");
        _isPathNotFound = true;
    }
}
