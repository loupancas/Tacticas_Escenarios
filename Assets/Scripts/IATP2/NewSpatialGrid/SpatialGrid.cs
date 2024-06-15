using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpatialGrid : MonoBehaviour
{
    private Dictionary<Vector2Int, List<EnemigoVolador>> _grid;

    private float _cellSize;

    public SpatialGrid(float cellSize)
    {
        _grid = new Dictionary<Vector2Int, List<EnemigoVolador>>();
        _cellSize = cellSize;
    }

    private Vector2Int GetCell(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / _cellSize);
        int y = Mathf.FloorToInt(position.y / _cellSize);
        return new Vector2Int(x, y);
    }

    public void AddEnemy(EnemigoVolador enemy)
    {
        Vector2Int cell = GetCell(enemy.transform.position);
        if (!_grid.ContainsKey(cell))
        {
            _grid[cell] = new List<EnemigoVolador>();
        }
        _grid[cell].Add(enemy);
    }

    public void RemoveEnemy(EnemigoVolador enemy)
    {
        Vector2Int cell = GetCell(enemy.transform.position);
        if (!_grid.ContainsKey(cell))
        {
            //_grid[cell].Remove(enemy);

            //if (_grid[cell].Count == 0)
            //{
            //    _grid.Remove(cell);
            //}
        }
    }

    public List<EnemigoVolador> GetEnemiesInRange(Vector3 position, float range)
    {
        List<EnemigoVolador> enemiesInRange = new List<EnemigoVolador>();
        Vector2Int centerCell = GetCell(position);
        int cellRange = Mathf.CeilToInt(range / _cellSize);

        for (int x = -cellRange; x <= cellRange; x++)
        {
            for (int y = -cellRange; y <= cellRange; y++)
            {
                Vector2Int cell = new Vector2Int(centerCell.x + x, centerCell.y + y);
                if (_grid.ContainsKey(cell))
                {
                    foreach (EnemigoVolador enemy in _grid[cell])
                    {
                        if (Vector3.Distance(position, enemy.transform.position) <= range)
                        {
                            enemiesInRange.Add(enemy);
                        }
                    }
                }
            }
        }
    
        return enemiesInRange;
    }

    //public void DrawDebugGrid()
    //{
    //    foreach (var cell in _grid)
    //    {
    //        Vector3 cellCenter = new Vector3(cell.Key.x * _cellSize + _cellSize / 2, cell.Key.y * _cellSize + _cellSize / 2, 0);
    //        Vector3 cellSize = new Vector3(_cellSize, _cellSize, 0);
    //        Debug.DrawLine(cellCenter - cellSize / 2, cellCenter + new Vector3(cellSize.x, -cellSize.y) / 2, Color.green);
    //        Debug.DrawLine(cellCenter - cellSize / 2, cellCenter + new Vector3(-cellSize.x, cellSize.y) / 2, Color.green);
    //        Debug.DrawLine(cellCenter + cellSize / 2, cellCenter - new Vector3(cellSize.x, cellSize.y) / 2, Color.green);
    //        Debug.DrawLine(cellCenter + cellSize / 2, cellCenter - new Vector3(-cellSize.x, -cellSize.y) / 2, Color.green);
    //    }
    //}

}
