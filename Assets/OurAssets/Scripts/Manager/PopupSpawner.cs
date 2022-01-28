using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float _spawnPeriod = 1f;
    [SerializeField] private Popup _popupBasePrefab;
    [SerializeField] private Color[] _popupColors;
    [Header("Spawn")]
    [SerializeField] private float _spawnPositionY = 5f;
    [SerializeField] private int _maxSimultaneousPopups = 10;
    [SerializeField, Range(0,1)] private float _spawnOnCursorProbability = 0.2f;

    private bool _shouldSpawn = false;
    private float _spawnTimer = 0f;
    private int _spawnedPopups = 0;

    private void Update()
    {
        // Early returns
        if (!_shouldSpawn) return;
        if (_spawnedPopups >= _maxSimultaneousPopups) return;

        // Spawn a popup every _spawnPeriod seconds
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnPeriod)
        {
            SpawnPopup();
            _spawnTimer = 0f;
        }
    }

    public void SetShouldSpawn(bool value) => _shouldSpawn = value;

    ///<summary>
    /// Instantiates and intiializes a new popup.
    ///</summary>
    private void SpawnPopup()
    {
        // Select a random color
        Color popupColor = _popupColors[Random.Range(0, _popupColors.Length)];

        // Select a position
        Vector3 spawnPosition = FindSpawnPosition();

        Popup newPopup = Instantiate<Popup>(_popupBasePrefab, spawnPosition, Quaternion.identity);
        newPopup.Initialize(popupColor);

        // Keep track of this popup util it is destroyed
        ++_spawnedPopups;
        newPopup.OnPopupDestroyed += PopupDespawned;
    }

    ///<summary>
    /// Returns a position for a popup to spawn at.
    ///</summary>
    private Vector3 FindSpawnPosition()
    {
        Vector3 spawnPos = Vector3.zero;
        bool spawnOnCursor = Random.Range(0f, 1f) < _spawnOnCursorProbability;
        if (spawnOnCursor)
        {
            Camera mainCamera = GameManager.Instance.MainCamera;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCamera.transform.position.y - _spawnPositionY;
            spawnPos = mainCamera.ScreenToWorldPoint(mousePos);
        }
        else
        {
            // Find a random position visible by the camera
            Camera mainCamera = GameManager.Instance.MainCamera;
            float zDepth = mainCamera.transform.position.y - _spawnPositionY;
            spawnPos = GameManager.Instance.MainCamera.RandomPointInFrustum(zDepth);
        }

        // Add a tiny variation on the Y axis to prevent overlap
        spawnPos.y += _spawnedPopups * 0.01f;

        return spawnPos;
    }

    ///<summary>
    /// Called when a Popup despawns.
    ///</summary>
    private void PopupDespawned(Popup popup)
    {
        --_spawnedPopups;
        popup.OnPopupDestroyed -= PopupDespawned;
    }
}