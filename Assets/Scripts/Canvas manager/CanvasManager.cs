using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private Canvas[] _menuCanvas;

    

    


    private void Start()
     {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        

         for(int i = 0; i < _menuCanvas.Length; i++)
         {
             if(i != 0)
             {
                 _menuCanvas[i].enabled = false;
             }
         }
     }

    public void EnableMenu(int menuToShow)
    {
        for (int i = 0; i < _menuCanvas.Length; i++)
        {
            if (i == menuToShow)
            {
                _menuCanvas[i].enabled = true;
            }
            else if (i != menuToShow && _menuCanvas[i].enabled)
            {
                _menuCanvas[i].enabled = false;
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quitted");
    }
}
