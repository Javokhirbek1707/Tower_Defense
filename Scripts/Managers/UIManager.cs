using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("UI Manager is NULL!");

            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
        UpdateStatus();
    }

    [SerializeField]
    private Text _warFundsText;
    [SerializeField]
    private Text _livesText;
    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private Text _statusText;
    [SerializeField]
    private GameObject _levelStatusPanel;
    [SerializeField]
    private Text _levelStatusText;

    public int warFunds = 500;
    public int lives = 20;

    void Update()
    {
        _warFundsText.text = "" + warFunds;
        _livesText.text = "" + lives;
    }

    public void AddWarFunds(int amount)
    {
        warFunds += amount;
    }

    public void SpendWarFunds(int amount)
    {
        warFunds -= amount;
    }

    public void LoseLife()
    {
        lives -= 1;

        if (lives <= 0)
        {
            lives = 0;
            ShowStatusPanel("GAME OVER YOU LOSE");
        }

        UpdateStatus();
    }

    public void UpdateWave(int waveNum)
    {
        _waveText.text = waveNum + "/10";
    }

    private void UpdateStatus()
    {
        if (lives > 12)
        {
            _statusText.text = "Good";
            _statusText.color = Color.blue;
        }
        else if (lives > 4)
        {
            _statusText.text = "Caution";
            _statusText.color = Color.yellow;
        }
        else
        {
            _statusText.text = "Critical";
            _statusText.color = Color.red;
        }
    }

    private void ShowStatusPanel(string message)
    {
        Time.timeScale = 0f;
        _levelStatusPanel.SetActive(true);
        _levelStatusText.text = message;
    }

    public void LevelComplete()
    {
        ShowStatusPanel("LEVEL COMPLETE");
    }
}
