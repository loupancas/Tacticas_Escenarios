using System.Collections.Generic;
using UnityEngine;

public class SquareQuery : MonoBehaviour, IQuery
{
    public SpatialGrid             targetGrid;
    public float                   width    = 15f;
    public float                   height   = 30f;
   

    public IEnumerable<IGridEntity> Query() {
        var h = height * 0.5f;
        var w = width * 0.5f;
        return targetGrid.Query(
            transform.position + new Vector3(-w, 0, -h),
            transform.position + new Vector3(w, 0, h),
            x => true);
    }

    private void OnDrawGizmos()
    {
        if (targetGrid == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 0, height));
    }
}