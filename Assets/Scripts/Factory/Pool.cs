using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pool<T>
{
    private Func<T> _factoryMethod = default;
    private Action<T, bool> _turnOnOffCallback = default;

    private List<T> _currentStonks = new List<T>();

    public Pool(Func<T> factoryMethod, Action<T, bool> callBack, int initialStock = 1)
    {
        _factoryMethod = factoryMethod;
        _turnOnOffCallback = callBack;
        
        for (int i = 0; i < initialStock; i++)
        {
            T obj = _factoryMethod();

            _turnOnOffCallback(obj, false);

            _currentStonks.Add(obj);
        }
    }

    public T GetObject()
    {
        var result = default(T);

        if(_currentStonks.Count > 0)
        {
            result = _currentStonks[0];
            _currentStonks.RemoveAt(0);
        }
        else
        {
            result = _factoryMethod.Invoke();
        }

        _turnOnOffCallback(result, true);

        return result;
    }

    public void ReturnObject(T obj)
    {
        _turnOnOffCallback(obj, false);
        _currentStonks.Add(obj);
    }
}
