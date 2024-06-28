using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScene : MonoBehaviour
{
    public void SceneManagment(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void Salir()
    {
        Application.Quit();
    }
}
