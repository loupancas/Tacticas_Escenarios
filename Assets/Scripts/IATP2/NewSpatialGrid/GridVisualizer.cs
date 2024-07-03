using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        var spatialGrid = GetComponent<SpatialGrid>();
        if(spatialGrid == null)
        {
            spatialGrid.OnDrawGizmos();
        }
    }
}
