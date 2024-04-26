using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Controles", menuName = "ScriptableObject/Controles")]
public class Controles : ScriptableObject
{
    [Range(1f, 500f)]public float mouseSensitivity = 100f;
    public KeyCode fireKey = KeyCode.Mouse0;
    public KeyCode meleeKey = KeyCode.F;
    public KeyCode stopTime = KeyCode.E;
    public KeyCode dashKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;
}
