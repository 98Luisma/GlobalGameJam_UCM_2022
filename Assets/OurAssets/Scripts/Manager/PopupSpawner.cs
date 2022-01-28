using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnPeriod = 1f;
    [SerializeField] private Popup _popupBasePrefab;
    [SerializeField] private Color[] _popupColors;

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
        // Select a random color
        Color popupColor = _popupColors[Random.Range(0, _popupColors.Length)];

        Popup newPopup = Instantiate<Popup>(_popupBasePrefab);
        newPopup.Initialize(popupColor);
    }

    public void SetShouldSpawn(bool value) => _shouldSpawn = value;
}