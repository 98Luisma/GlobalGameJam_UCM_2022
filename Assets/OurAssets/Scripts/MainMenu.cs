using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}