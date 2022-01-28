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

    [Header("Map")]
    [SerializeField] private float _gameAreaWidth = 10f;
    [SerializeField] private float _gameAreaHeight = 10f;

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

#region Getters
    public Camera MainCamera { get => _mainCamera; }
    public float GameAreaWidth { get => _gameAreaWidth; }
    public float GameAreaHeight { get => _gameAreaHeight; }
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
        _popupSpawner.SetShouldSpawn(true);
        // _enemySpawner.SetShouldSpawn(true);
    }
}