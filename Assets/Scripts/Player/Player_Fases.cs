using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Fases 
{
    public int fases = 0;
    CountdownTimer _timer;
    float _timeFaseToFase;
    TextoActualizable _text;
    public Player_Fases(float timer, TextoActualizable texto)
    {
        _timeFaseToFase = timer;
        _timer = new CountdownTimer(timer);
        _timer.OnTimerStop = BajarFase;
        _text = texto;
    }

    public void BajarFase()
    {
        Debug.Log("Fase: " + fases);
        if (fases <= 0)
            return;

        fases--;
        _text.UpdateHUD(fases, "Fase");

        _timer.Reset(_timeFaseToFase);
        _timer.Start();
        GameManager.instance.pj.equippedWeapon.UpdateFase(fases);
    }

    public void SubirFase()
    {
        if (fases >= 3)
            return;


        fases++;
        Debug.Log("Fase: " + fases);
        _text.UpdateHUD(fases, "Fase");
        _timer.Reset(_timeFaseToFase);
        _timer.Start();
        GameManager.instance.pj.equippedWeapon.UpdateFase(fases);
    }
    
    public void UpdateTimer(float deltatime)
    {
        _timer.Tick(deltatime);
    }
}
