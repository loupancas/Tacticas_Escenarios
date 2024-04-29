using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModifierStat
{
    Dictionary<string, Buff> buffs = new();

    public Stats StatOriginal, StatResultado;

    

    public ModifierStat(Stats stats)
    {
        StatOriginal = stats;

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

}
