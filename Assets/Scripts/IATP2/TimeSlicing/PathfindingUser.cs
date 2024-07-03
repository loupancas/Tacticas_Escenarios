using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class PathfindingUser : MonoBehaviour
{
    [SerializeField] Node startNode = null;
    [SerializeField] Node endNode = null;
    private AStar<Node> _aStar;

    private void Start()
    {
        _aStar = new AStar<Node>();
        _aStar.OnPathCompleted += GetPath;
        _aStar.OnCantCalculate += PathNotFound;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(_aStar.Run(startNode,IsGoal,Explode,GetHeuristic));
        }
    }

    private float GetHeuristic(Node node)
    {
        return node.heuristic;
    }

    private IEnumerable<WeightedNode<Node>> Explode(Node node)
    {
        return node.neighbour.Select(neighbour => new WeightedNode<Node>(neighbour, 1f));
    }

    private bool IsGoal(Node node)
    {
        return node == endNode;
    }

    void GetPath(IEnumerable<Node> path)
    {
        foreach (Node node in path)
        {
            Debug.Log(node.name);
            node.isPath = true;
        }
    }

    void PathNotFound()
    {
        Debug.Log("Path not found");
    }
}
