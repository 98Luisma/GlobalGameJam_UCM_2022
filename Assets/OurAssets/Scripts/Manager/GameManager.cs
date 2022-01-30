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

    [Header("HUD")]
    [SerializeField] private Canvas _mainCanvas = null;
    [SerializeField] private TextMeshProUGUI scoreUI = null;
    [SerializeField] private TextMeshProUGUI moneyUI = null;
    [SerializeField] private Transform _lifeLayoutGroup = null;

    [Header("Player")]
    [SerializeField] private int _maxLife = 5;
    [SerializeField] private Image _lifeImagePrefab = null;
    [SerializeField] private int _initMoney = 1000;

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    private int life;
    private int money;
    private int score;
    private List<Image> _livesDisplay;

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

        life = _maxLife;
        money = _initMoney;
        score = 0;
        DisplayMoney();
        DisplayScore();

        VolatileStorage.GetInstance().lives = life;
        VolatileStorage.GetInstance().money = money;
        VolatileStorage.GetInstance().score = score;
        VolatileStorage.GetInstance().causeoOfDeath = VolatileStorage.CauseOfDeath.None;

        // Spawn the lives images
        _livesDisplay = new List<Image>(_maxLife);
        for (int i=0; i<_maxLife; ++i)
        {
            Image newImage = Instantiate(_lifeImagePrefab, _lifeLayoutGroup);
            _livesDisplay.Add(newImage);
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        money = Mathf.Max(0, money);
        DisplayMoney();
        VolatileStorage.GetInstance().money = money;
        
        if (money <= 0)
        {
            VolatileStorage.GetInstance().causeoOfDeath = VolatileStorage.CauseOfDeath.Money;
            KillPlayer();
        }
    }

    public void AddLife(int amount)
    {
        life += amount;
        life = Mathf.Max(0, life);
        VolatileStorage.GetInstance().lives = life;

        // Debug.Log(life);
        if (life <= 0)
        {
            VolatileStorage.GetInstance().causeoOfDeath = VolatileStorage.CauseOfDeath.Life;
            KillPlayer();
        }
        else
        {
            for (int i = 0; i < _maxLife; ++i)
            {
                if (i <= life-1)
                    _livesDisplay[i].color = Color.white;
                else
                    _livesDisplay[i].color = Color.black;
            }
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        score = Mathf.Max(0, score);
        DisplayScore();
        VolatileStorage.GetInstance().score = score;
    }

    public EnemyBullet RequestEnemyBullet()
    {
        return _enemySpawner.RequestEnemyBullet();
    }

    private void KillPlayer()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoseMenu");
    }

    private void DisplayMoney()
    {
        moneyUI.text = "Money: " + money;
    }

    private void DisplayScore()
    {
        scoreUI.text = "Kills: " + score;
    }
}