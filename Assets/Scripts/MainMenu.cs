﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenLevel1()
    {
        SceneManager.LoadScene("Level1");        
    }
    public void OpenLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}