using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleArena : ArenaBase
{
    [SerializeField] Puerta puerta;
    public override void IniciarHorda()
    {
        GameManager.instance.arenaManager = this;
        if (puerta != null)
        {
            puerta.gameObject.SetActive(true);
        }

        _arenaEmpezada = true;
        foreach (SpawnPoints a in spawnPoints)
        {
            a.SpawnEnemy();
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemigosEnLaArena.Count == 0 && _arenaEmpezada)
        {
            if (puerta != null)
                puerta.Desbloquear();
        }
    }
}
