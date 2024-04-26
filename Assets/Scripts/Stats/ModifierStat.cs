using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ModifierStat
{
    Dictionary<string, Buff> buffs = new();

    public Stats original;

    public Stats resultado;

    public void Remove(string a)
    {
        buffs.Remove(a);

        UpdateBuffs();
    }

    public void Add(string s, Func<Stats, Stats> func, float time)
    {
        buffs.Add(s, new Buff(time, () => Remove(s), func));
        UpdateBuffs();
    }

    public void Update()
    {
        foreach(var (s,buff) in buffs)
        {
            buff.Update(Time.deltaTime);
        }
    }

    void UpdateBuffs()
    {
        resultado = original;

        foreach (var (s, buff) in buffs)
        {
            resultado = buff.calculate(resultado);
        }
    }

}
