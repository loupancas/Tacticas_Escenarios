using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node : MonoBehaviour
{
    public List<Node> neighbour  = new List<Node>();
    public int heuristic = 1;

    public bool isPath;

    private void Awake()
    {
        neighbour = Physics.OverlapSphere(transform.position, 1.5f).Select(x => x.GetComponent<Node>()).Where(x => x != null).
            Where(x => x.gameObject != gameObject).ToList();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isPath ? Color.green : Color.red;

        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
