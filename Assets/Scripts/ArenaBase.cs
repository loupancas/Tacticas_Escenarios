using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArenaBase : MonoBehaviour
{
    public EnemigoBase[] enemigos;
    public SpawnPoints[] spawnPoints;
    public List<Node> nodos = new List<Node>();
    public List<EnemigoBase> enemigosEnLaArena;
    [SerializeField]protected bool _arenaEmpezada;
    [SerializeField] float _distanceToDisapier = 10;
    LayerMask _maskWall;

    public abstract void IniciarHorda();
    public Node GetMinNode(Vector3 position)
    {
        print("Funciono");
        Node minNode = null;
        float minDist = Mathf.Infinity;

        for (int i = 0; i < nodos.Count; i++)
        {
            if (InLineOfSight(nodos[i].transform.position, position))
            {
                if (Vector3.Distance(nodos[i].transform.position, position) < minDist)
                {
                    minNode = nodos[i];
                    minDist = Vector3.Distance(nodos[i].transform.position, position);
                }
            }
        }

        if (minDist > _distanceToDisapier)
            return null;

        return minNode;
    }

    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, _maskWall);
    }
}
