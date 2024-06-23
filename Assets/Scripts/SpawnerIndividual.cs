using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerIndividual : SpawnPoints
{
    
    public override void SpawnEnemy()
    {
        enemigo.SpawnEnemy(transform);
    }
    
}
