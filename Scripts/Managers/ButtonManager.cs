using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    private static ButtonManager _instance;
    public static ButtonManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Button Manager is NULL!");
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    [SerializeField]
    private GameObject _optionsMenu;

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void PauseButton()
    {
        Time.timeScale = 0f;
        Debug.Log("Game paused");
    }

    public void PlayButton()
    {
        Time.timeScale = 1f;
        Debug.Log("Play is played");
    }

    public void DoubleSpeed()
    {
        Time.timeScale = 2f;
        Debug.Log("Doublespeed is played");
    }




    /// THIS BUTTONS FOR INTRO SCENE

    public void GamePlay()
    {
        SceneManager.LoadScene(1);
    }

    public void OptionButton()
    {
        _optionsMenu.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void CloseButton()
    {
        _optionsMenu.SetActive(false);
    }
}
