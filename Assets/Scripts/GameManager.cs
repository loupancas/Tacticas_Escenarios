using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Components")]
    public FirstPersonPlayer pj;
    public Arena arenaManager;
    [SerializeField] private float distanceThreshold = 20f;
    [SerializeField] private float checkInterval = 3f;
    private float timeSinceLastCheck = 0f;
    public List<EnemigoVolador> enemies;
    public bool updateList = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            EnemigoVolador.InitializeGrid();
        }


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
        if(updateList)
        {
            enemies = new List<EnemigoVolador>(FindObjectsOfType<EnemigoVolador>());
            updateList = false;
        }

        EnemigoVolador.DeactivateEnemiesByDistance(pj,enemies, distanceThreshold);
    }

    private void FuseNearbyEnemies()
    {
        if (updateList)
        {
            enemies = new List<EnemigoVolador>(FindObjectsOfType<EnemigoVolador>());
            updateList = false;
        }
        foreach (var enemy in enemies)
        {
            var cellPosition = EnemigoVolador._spatialGrid.GetPositionInGrid(enemy.Position);
            EnemigoVolador.FuseEnemiesInRange(enemy.Position, EnemigoVolador._spatialGrid.cellWidth);
        }
    }
}
