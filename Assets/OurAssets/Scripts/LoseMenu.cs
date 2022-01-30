using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoseMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _deathText = null;
    [SerializeField] private string _msgOnDead = "You died!";
    [SerializeField] private string _msgOnBroke = "You ran out of money!";
    [SerializeField] private TextMeshProUGUI _scoreText = null;

    private void Awake()
    {
        UpdateDeathMessage();
        UpdateScore();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void UpdateDeathMessage()
    {
        switch (VolatileStorage.GetInstance().causeoOfDeath)
        {
            case VolatileStorage.CauseOfDeath.Life:
            {
                _deathText.text = _msgOnDead;
                break;
            }
            case VolatileStorage.CauseOfDeath.Money:
            {
                _deathText.text = _msgOnBroke;
                break;
            }
            default:
            {
                _deathText.text = "You shouldn't be here!";
                break;
            }
        }
    }

    private void UpdateScore()
    {
        _scoreText.text = "Killed enemies: " + VolatileStorage.GetInstance().score;
    }
}