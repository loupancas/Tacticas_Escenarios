
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
public class ResumeGameStop : MonoBehaviour
{
    public GameObject setting;
    public bool isSettingActive;
    private bool isPaused = false;


    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        isSettingActive = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isSettingActive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (isPaused)
            {
                ResumeGame();
                if (!isSettingActive)
                {
                    Resume();
                }

            }
            else
            {
                PauseGame();
                if (isSettingActive)
                {
                    Pause();
                }

            }
        }
    }

    public void Pause ()
    {
        setting.SetActive(true);
        //isSettingActive = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        this.GetComponent<FirstPersonCamera>().enabled = false;

    }

    public void Resume ()
    {
        setting.SetActive(false);
        //isSettingActive = false;

        Cursor.lockState = CursorLockMode.None;
        this.GetComponent<FirstPersonCamera>().enabled = true;
        Cursor.visible = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}