using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModifierStat
{
    Dictionary<string, Buff> buffs = new();

    public Stats StatOriginal, StatResultado;

    public WeaponBase ArmaOriginal, ArmaResultado;

    public ModifierStat(Stats stats)
    {
        StatOriginal = stats;
        
    }

    public void ArmaUpdate(WeaponBase Weaponstats)
    {
        ArmaOriginal = Weaponstats;
        Debug.Log("Arma Actualizada: " + ArmaOriginal);
    }

    public void Remove(string a)
    {
        buffs.Remove(a);

        UpdateBuffs();
    }

    public void Add(string s, Func<Stats, Stats> func, float time)
    {   
        if(!buffs.ContainsKey(s))
        {
           buffs.Add(s, new Buff(time, () => Remove(s), func));
        }
        
        UpdateBuffs();
    }

    public void Add(string s, Func<WeaponBase, WeaponBase> func, float time)
    {
        if (!buffs.ContainsKey(s))
        {
            buffs.Add(s, new Buff(time, () => Remove(s), func));
        }

        UpdateArmaBuffs();
    }

    public void Update()
    {
        foreach(var (s,buff) in buffs)
        {
            buff.Update(Time.deltaTime);
        }
    }

    public void UpdateBuffs()
    {
        StatResultado = StatOriginal;
        

        foreach (var (s, buff) in buffs)
        {
            StatResultado = buff.calculate(StatResultado);
            
        }
    }

    public void UpdateArmaBuffs()
    {
        ArmaResultado = ArmaOriginal;

        foreach(var (s,buff) in buffs)
        {
            ArmaResultado = buff.CalculateWeapon(ArmaOriginal);
        }
    }

}
