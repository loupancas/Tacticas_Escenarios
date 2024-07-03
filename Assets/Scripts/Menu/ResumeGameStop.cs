using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeGameStop : MonoBehaviour
{
    public GameObject setting;
    public bool isSettingActive;

    private bool isPaused = false;


    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (isPaused)
            {
                ResumeGame();
                if (isSettingActive)
                {
                    Resume();
                }
            }
            else
            {
                PauseGame();
                if (!isSettingActive)
                {
                    Pause();
                }
            }
        }
    }

    public void Pause ()
    {
        setting.SetActive(true);
        isSettingActive = true;
        //this.GetComponent<PlayerLook>().enabled = false;

        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume ()
    {
        setting.SetActive(false);
        isSettingActive = false;
        //this.GetComponent<PlayerLook>().enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
    }
}