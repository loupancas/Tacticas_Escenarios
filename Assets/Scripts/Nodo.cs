using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo : MonoBehaviour
{
    public List<Nodo> nodosVecinos = new List<Nodo>();
    public Arena arena;

    CountdownTimer _timer;

    [SerializeField]LayerMask _wallMask;
    private void Start()
    {
        arena.nodos.Add(this);

        _timer = new CountdownTimer(0.1f);

        
    }

    private void Update()
    {
        _timer.Tick(Time.deltaTime);

        SetearNodosVecinos();
    }

    public void SetearNodosVecinos()
    {
        nodosVecinos.Clear();
        foreach (Nodo nodo in arena.nodos)
        {
            if (nodo == this) continue;

            if (InLineOfSight(transform.position, nodo.transform.position))
            {
                nodosVecinos.Add(nodo);
            }
        }
    }
    protected bool InLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, _wallMask);
    }
}
