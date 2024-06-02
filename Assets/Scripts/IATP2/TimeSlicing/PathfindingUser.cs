using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class PathfindingUser : MonoBehaviour
{
    [SerializeField] Node startNode = null;
    [SerializeField] Node endNode = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //StartCoroutine(AStar.CalculatePath(startNode, (x) => x == endNode, x => x.neighbour.Select(x => new WeightedNode<Node>(x, 1)),
            //        x => x.heuristic, GetPath));
        }
    }

    void GetPath(IEnumerable<Node> path)
    {
        foreach (Node node in path)
        {
            Debug.Log(node.name);
            node.isPath = true;
        }
    }
}
