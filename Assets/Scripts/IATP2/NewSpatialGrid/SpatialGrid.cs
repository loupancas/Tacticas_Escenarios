using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class SpatialGrid : MonoBehaviour
{
    public float x= 0;
    public float z = 0;
    public float cellWidth=10f;
    public float cellHeight = 10f;
    public int width = 20;
    public int height = 20;

    private Dictionary<IGridEntity, Tuple<int, int>> lastPositions=new Dictionary<IGridEntity, Tuple<int, int>>();
    private HashSet<IGridEntity>[,] buckets;
    readonly public Tuple<int, int> Outside =  Tuple.Create(-1, -1);
    readonly public IGridEntity[] _empty = new IGridEntity[0];

    private void Awake()
    {
        InitializeBuckets();
        var entities = GetComponentsInChildren<IGridEntity>();
        foreach (var entity in entities)
        {
            entity.OnMove += UpdateEntity;
        }
    }
    private void InitializeBuckets()
    {
        buckets = new HashSet<IGridEntity>[width, height];
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                buckets[i, j] = new HashSet<IGridEntity>();
            }
        }
    }

    public void Add(IGridEntity entity)
    {
        entity.OnMove += UpdateEntity;
        UpdateEntity(entity);
    }

    public void Remove(IGridEntity entity)
    {
        entity.OnMove -= UpdateEntity;
        UpdateEntity(entity);
        var currentPos = GetPositionInGrid(entity.Position);
        if (IsInsideGrid(currentPos))
        {
            buckets[currentPos.Item1, currentPos.Item2].Remove(entity);
        }
        lastPositions.Remove(entity);
    }

    public void UpdateEntity(IGridEntity entity)
    {
        var lastPos = lastPositions.ContainsKey(entity) ? lastPositions[entity] : Outside;
        var currentPos = GetPositionInGrid(entity.Position);

        if (lastPos.Equals(currentPos))
            return;

        if (IsInsideGrid(lastPos))
        {
            buckets[lastPos.Item1, lastPos.Item2].Remove(entity);
        }

        if (IsInsideGrid(currentPos))
        {
            buckets[currentPos.Item1, currentPos.Item2].Add(entity);
            lastPositions[entity] = currentPos;
        }
        else
        {
            lastPositions.Remove(entity);
        }
    }

    public IEnumerable<IGridEntity> GetEntitiesInCell(Tuple<int, int> cellPosition)
    {
        if (IsInsideGrid(cellPosition))
        {
            return buckets[cellPosition.Item1, cellPosition.Item2];
        }
        return _empty;
    }

    public IEnumerable<IGridEntity> Query(Vector3 aabbFrom, Vector3 aabbTo, Func<Vector3, bool> filterByPosition)
    {
        var from = new Vector3(Mathf.Min(aabbFrom.x, aabbTo.x), 0, Mathf.Min(aabbFrom.z, aabbTo.z));
        var to = new Vector3(Mathf.Max(aabbFrom.x, aabbTo.x), 0, Mathf.Max(aabbFrom.z, aabbTo.z));

        var fromCoord = GetPositionInGrid(from);
        var toCoord = GetPositionInGrid(to);

        fromCoord = Tuple.Create(Clamp(fromCoord.Item1, 0, width - 1), Clamp(fromCoord.Item2, 0, height - 1));
        toCoord = Tuple.Create(Clamp(toCoord.Item1, 0, width - 1), Clamp(toCoord.Item2, 0, height - 1));

        if (!IsInsideGrid(fromCoord) && !IsInsideGrid(toCoord))
            return _empty;

        var cols = Enumerable.Range(fromCoord.Item1, toCoord.Item1 - fromCoord.Item1 + 1);
        var rows = Enumerable.Range(fromCoord.Item2, toCoord.Item2 - fromCoord.Item2 + 1);

        var cells = cols.SelectMany(col => rows.Select(row => Tuple.Create(col, row)));

        return cells
            .SelectMany(cell => buckets[cell.Item1, cell.Item2])
            .Where(e => from.x <= e.Position.x && e.Position.x <= to.x && from.z <= e.Position.z && e.Position.z <= to.z)
            .Where(e => filterByPosition(e.Position));
    }

    public Tuple<int, int> GetPositionInGrid(Vector3 pos)
    {
        return Tuple.Create(Mathf.FloorToInt((pos.x - x) / cellWidth), Mathf.FloorToInt((pos.z - z) / cellHeight));
    }

    public bool IsInsideGrid(Tuple<int, int> position)
    {
        return 0 <= position.Item1 && position.Item1 < width && 0 <= position.Item2 && position.Item2 < height;
    }

    private int Clamp(int value, int min, int max)
    {
        return Mathf.Clamp(value, min, max);
    }

    private void OnDestroy()
    {
        var entities = GetComponentsInChildren<IGridEntity>();
        foreach (var e in entities)
        {
            e.OnMove -= UpdateEntity;
        }
    }

    public void OnDrawGizmos()
    {
        DrawGrid();
    }

    private void DrawGrid()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i <= width; i++)
        {
            Gizmos.DrawLine(new Vector3(x + i * cellWidth, 0, z), new Vector3(x + i * cellWidth, 0, z + height * cellHeight));
        }

        for (int j = 0; j <= height; j++)
        {
            Gizmos.DrawLine(new Vector3(x, 0, z + j * cellHeight), new Vector3(x + width * cellWidth, 0, z + j * cellHeight));
        }
    }






}
