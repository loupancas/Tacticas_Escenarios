using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFreezed 
{

    public void StoppedTime();
    public void NormalUpdate();
    public void Freezed();
    public IEnumerator StopTime();

}
