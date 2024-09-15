using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool _isPaused = false;
    [SerializeField] private GameObject _pauseUI;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (_isPaused)
                Resume();
            else
                Pause();
        } 
    }

    public void Resume()
    {
        _pauseUI.SetActive(false);
        Time.timeScale = 1;
        _isPaused = false;
    }

    private void Pause()
    {
        _pauseUI.SetActive(true);
        Time.timeScale = 0;
        _isPaused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
    }
}
