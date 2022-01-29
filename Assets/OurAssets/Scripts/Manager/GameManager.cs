using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private PopupSpawner _popupSpawner = null;
    [SerializeField] private EnemySpawner _enemySpawner = null;

    [Header("References")]
    [SerializeField] private Camera _mainCamera = null;
    [SerializeField] private PlayerController _player = null;

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    private int life;
    private int money;

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
        DontDestroyOnLoad(gameObject);
        // End of singleton implementation
    }

    private void Start()
    {
        // _popupSpawner.SetShouldSpawn(true);
        // _enemySpawner.SetShouldSpawn(true);
        life = 10;
        money = 1000;
    }

    void addMoney(int amount)
    {
        money += amount;
    }

    void addLife(int amount)
    {
        life += amount;
    }
}