using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    public virtual void Desbloquear()
    {
        gameObject.SetActive(false);
    }
    
}
