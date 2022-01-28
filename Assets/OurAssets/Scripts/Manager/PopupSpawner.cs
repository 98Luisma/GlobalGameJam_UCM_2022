using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSpawner : MonoBehaviour
{
    [SerializeField] private Popup[] _popupPrefabs;
    [SerializeField] private float _spawnPeriod = 1f;

    private bool _shouldSpawn = false;
    private float _spawnTimer = 0f;

    private void Update()
    {
        if (!_shouldSpawn) return;
        
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnPeriod)
        {
            SpawnPopup();
            _spawnTimer = 0f;
        }
    }

    private void SpawnPopup()
    {

    }

    public void SetShouldSpawn(bool value) => _shouldSpawn = value;
}