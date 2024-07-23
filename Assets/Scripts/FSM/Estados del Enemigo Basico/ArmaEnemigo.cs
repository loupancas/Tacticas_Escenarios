using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaEnemigo : MonoBehaviour
{
    


    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 dir = new Vector3(GameManager.instance.pj.transform.position.x, GameManager.instance.pj.transform.position.y, GameManager.instance.pj.transform.position.z);

        transform.LookAt(dir);
    }
}
