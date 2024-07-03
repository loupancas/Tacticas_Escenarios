using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kinpy : Entity
{

    public override void Morir()
    {
        gameObject.SetActive(false);
    }
   

}
