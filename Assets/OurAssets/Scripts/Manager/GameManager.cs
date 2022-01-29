using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private PopupSpawner _popupSpawner = null;
    [SerializeField] private EnemySpawner _enemySpawner = null;
    [SerializeField] private BackgroundManager _backgroundManager = null;

    [Header("References")]
    [SerializeField] private Camera _mainCamera = null;
    [SerializeField] private PlayerController _player = null;

    [SerializeField] private Canvas _mainCanvas = null;
    [SerializeField] private TextMeshProUGUI scoreUI = null;
    [SerializeField] private TextMeshProUGUI moneyUI = null;
    private Transform lives = null;

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    private int life;
    private int money;
    private int score;

#region Getters
    public Camera MainCamera { get => _mainCamera; }
    public PlayerController Player { get => _player; }
#endregion

    private void Awake()
    {
        // Singleton implementation
        if (_instance && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        // DontDestroyOnLoad(gameObject);
        // End of singleton implementation
    }

    private void Start()
    {
        _popupSpawner.SetShouldSpawn(true);
        _enemySpawner.SetShouldSpawn(true);

        life = 5;
        money = 1000;
        moneyUI.text = money.ToString();
        score = 0;
        scoreUI.text = score.ToString();

        lives = _mainCanvas.transform.Find("Lives");
    }

    public void addMoney(int amount)
    {
        money += amount;
        moneyUI.text = money.ToString();
    }

    public void addLife(int amount)
    {
        life += amount;
        int children = lives.childCount;

        for (int i = 0; i < children; ++i)
        {
            if (i < life)
                lives.transform.GetChild(i).GetComponent<RawImage>().color = Color.white;
            else
                lives.transform.GetChild(i).GetComponent<RawImage>().color = Color.black;
        }

        //_mainCanvas.transform.Find("Lives").Find("Hearth" + totalLives).gameObject.GetComponent<RawImage>().color = Color.black;
    }

    public void addScore(int amount)
    {
        score += amount;
        scoreUI.text = score.ToString();
    }
}