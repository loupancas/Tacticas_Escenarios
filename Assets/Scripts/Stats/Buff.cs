using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Buff
{
    public event Action OnFinished;
    readonly CountdownTimer timer;

    public Func<Stats, Stats> calculate;

    public Buff(float duration, Action action, Func<Stats, Stats> calculate)
    {
        if (duration <= 0) return;
        timer = new CountdownTimer(duration);
        timer.Start();
        OnFinished += action;
        this.calculate = calculate;
    }

    public void Update(float deltaTime)
    {
        if (timer.IsFinished)
        {
            OnFinished?.Invoke();
            return;
        }
        timer?.Tick(deltaTime);
    }
}
