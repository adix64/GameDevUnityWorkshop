using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGameCtrl : MonoBehaviour
{
    public bool gamePaused = false;

    public GameObject pausePanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            if (gamePaused)
                PauseGame();
            else
                Resume();
        }

    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gamePaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        gamePaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }
    public void QuitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
