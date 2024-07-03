using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextoActualizable : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    public void UpdateHUD(int NumeroActual, int NumeroMaximo, string Texto)
    {
        _text.text = NumeroActual + " / " + NumeroMaximo + " " + Texto;
    }
    public void UpdateHUD(float NumeroActual, float NumeroMaximo, string Texto)
    {
        _text.text = NumeroActual + " / " + NumeroMaximo + " " + Texto;
    }
    public void UpdateHUD(int Numero, string Texto)
    {
        _text.text = Numero + " " + Texto;
    }
    public void UpdateHUD(float Numero, string Texto)
    {
        _text.text = Numero + " " + Texto;
    }
    public void UpdateHUD(float Numero)
    {
        _text.text = Numero + "";
    }
    public void UpdateHUD(int Numero)
    {
        _text.text = Numero + "";
    }
}
