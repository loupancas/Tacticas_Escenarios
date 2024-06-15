using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;    

public class HealthBar : MonoBehaviour
{
    Image _hPBar;

    private void Start()
    {
        _hPBar = gameObject.GetComponent<Image>();
    }

    public void UpdateHPBar(int vidaActual)
    {
        _hPBar.fillAmount = vidaActual / 100f; 
    }

}
