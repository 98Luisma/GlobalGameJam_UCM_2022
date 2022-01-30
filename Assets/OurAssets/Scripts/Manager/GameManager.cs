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
    [SerializeField] private AudioManager _audioManager = null;

    [Header("References")]
    [SerializeField] private Camera _mainCamera = null;
    [SerializeField] private PlayerController _player = null;

    [Header("HUD")]
    [SerializeField] private TextMeshProUGUI scoreUI = null;
    [SerializeField] private TextMeshProUGUI moneyUI = null;
    [SerializeField] private Transform _lifeLayoutGroup = null;
    [SerializeField] private float _moneyAnimationSeconds = 0.2f;
    [SerializeField] private Color _moneyAnimationGoodColor = Color.green;
    [SerializeField] private Color _moneyAnimationBadColor = Color.red;

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
    float _initMoneyFontSize;

    private Coroutine _moneyAnimationCoroutine = null;

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

        _initMoneyFontSize = moneyUI.fontSize;
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

        OnGameStarted();
    }

    public EnemyBullet RequestEnemyBullet()
    {
        return _enemySpawner.RequestEnemyBullet();
    }

#region Points methods
    public void AddMoney(int amount)
    {
        money += amount;
        money = Mathf.Max(0, money);
        DisplayMoney();
        VolatileStorage.GetInstance().money = money;

        // Play an animation
        if (amount > 0)
        {
            if (_moneyAnimationCoroutine != null)
            {
                StopCoroutine(_moneyAnimationCoroutine);
                moneyUI.fontSize = _initMoneyFontSize;
                moneyUI.color = Color.white;
            }
            _moneyAnimationCoroutine = StartCoroutine(AddMoneyAnimation(true));
        }
        else if (amount < 0) // Don't do it for amount == 0
        {
            if (_moneyAnimationCoroutine != null)
            {
                StopCoroutine(_moneyAnimationCoroutine);
                moneyUI.fontSize = _initMoneyFontSize;
                moneyUI.color = Color.white;
            }
            _moneyAnimationCoroutine = StartCoroutine(AddMoneyAnimation(false));
        }
        
        // Lose the game?
        if (money <= 0)
        {
            VolatileStorage.GetInstance().causeoOfDeath = VolatileStorage.CauseOfDeath.Money;
            KillPlayer();
        }
    }

    private IEnumerator AddMoneyAnimation(bool good)
    {
        moneyUI.color = good ? _moneyAnimationGoodColor : _moneyAnimationBadColor;

        float timer = 0f;
        while (timer <= _moneyAnimationSeconds)
        {
            float angle = Mathf.PI * timer / _moneyAnimationSeconds;
            moneyUI.fontSize = _initMoneyFontSize + 16f * Mathf.Sin(angle);
            
            timer += Time.deltaTime;
            yield return null;
        }

        moneyUI.fontSize = _initMoneyFontSize;
        moneyUI.color = Color.white;
    }

    public void AddLife(int amount)
    {
        life += amount;
        life = Mathf.Max(0, life);
        life = Mathf.Min(life, _maxLife);
        VolatileStorage.GetInstance().lives = life;

        if (amount < 0)
        {
            OnPlayerDamaged();
        }
        else if (amount > 0)
        {
            OnPlayerHealthRestored();
        }

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
#endregion

#region Events for audio
    public void OnAllPopupsDespawned()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Mute, AudioManager.SoundType.AdsMusic);
        _audioManager.ManageAudio(AudioManager.AudioAction.ToForeground, AudioManager.SoundType.GameMusic);
    }

    public void OnEnemyBulletSpawned()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.EnemyBullet);
    }

    public void OnPlayerBulletSpawned()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.PlayerBullet);

    }

    public void OnPlayerBulletExplosion()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.Explosion);
        
    }

    public void OnBossBulletSpawned()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.BossBullet);

    }

    public void OnEnemyDestroyed()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.EnemyDestroyed);

    }

    public void OnPlayerDamaged()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.Damaged);

    }

    public void OnBossSpawned()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Stop, AudioManager.SoundType.GameMusic);
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.BossMusic);
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.BossTurbines);
    }

    public void OnBossDefeated()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Stop, AudioManager.SoundType.BossMusic);
        _audioManager.ManageAudio(AudioManager.AudioAction.Stop, AudioManager.SoundType.BossTurbines);
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.GameMusic);
    }

    public void OnPlayerHealthRestored()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.LifeRestored);
    }
    public void OnFirstPopupOpen()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.ToBackground, AudioManager.SoundType.GameMusic);
        _audioManager.ManageAudio(AudioManager.AudioAction.UnMute, AudioManager.SoundType.AdsMusic);
    }
    public void OnPopupOpen()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.PopupOpen);
    }

    public void OnPopupClosed()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.PopupClose);
    }

    public void OnPopupBought()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.PopupBuy);
    }

    public void OnDifficultyIncreased()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Next, AudioManager.SoundType.AdsMusic);
    }

    public void OnGameStarted()
    {
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.GameMusic);
        _audioManager.ManageAudio(AudioManager.AudioAction.Mute, AudioManager.SoundType.AdsMusic);
        _audioManager.ManageAudio(AudioManager.AudioAction.Play, AudioManager.SoundType.AdsMusic);
    }
#endregion
}