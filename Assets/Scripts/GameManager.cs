using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Components")]
    public FirstPersonPlayer pj;
    public ArenaBase arenaManager;
    public void Awake()
    {
        if (instance == null)
            instance = this;

<<<<<<< Updated upstream

    }
    private void Start()
    {
    }
    private void Update()
    {
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck >= checkInterval)
        {
            timeSinceLastCheck = 0f;
            //UpdateDistanceEnemies();
            FuseNearbyEnemies();
        }
    }

    private void UpdateDistanceEnemies()
    {
        if (updateList)
        {
            enemies = new List<EnemigoVolador>(FindObjectsOfType<EnemigoVolador>());
            updateList = false;
        }

        EnemigoVolador.DeactivateEnemiesByDistance(pj, enemies, distanceThreshold);
    }

    private void FuseNearbyEnemies()
    {
        if (updateList)
        {
            enemies = new List<EnemigoVolador>(FindObjectsOfType<EnemigoVolador>());
            updateList = false;
        }

        // Iterar sobre cada enemigo y tratar de fusionarlo con enemigos en la misma celda
        foreach (var enemigo in enemies)
        {
            var cellPosition = EnemigoVolador._spatialGrid.GetPositionInGrid(enemigo.Position);
            EnemigoVolador.FuseEnemiesInRange(enemigo.Position, EnemigoVolador._spatialGrid.cellWidth); // Usa el tamaño de la celda para el rango
        }
=======
>>>>>>> Stashed changes
    }
}