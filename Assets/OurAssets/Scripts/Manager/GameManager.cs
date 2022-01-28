using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private PopupSpawner _popupSpawner = null;
    [SerializeField] private EnemySpawner _enemySpawner = null;

    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }

    private void Awake()
    {
        // Singleton implementation
        if (_instance && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        // End of singleton implementation
    }

    private void Start()
    {
        _popupSpawner.SetShouldSpawn(true);
        _enemySpawner.SetShouldSpawn(true);
    }
}