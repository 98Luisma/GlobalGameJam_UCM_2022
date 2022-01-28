using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnPeriod = 1f;

    private bool _shouldSpawn = false;
    private float _spawnTimer = 0f;

    private void Update()
    {
        if (!_shouldSpawn) return;
        
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnPeriod)
        {
            SpawnEnemy();
            _spawnTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {

    }

    public void SetShouldSpawn(bool value) => _shouldSpawn = value;
}
