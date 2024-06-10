using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Components")]
    public FirstPersonPlayer pj;
    public ArenaBase arenaManager;
    public void Awake()
    {
        if (instance == null)
            instance = this;

    }
}
