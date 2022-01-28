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
        Vector3 spawnPosition = spawnPosition = FindSpawnPosition();

        Popup newPopup = Instantiate<Popup>(_popupBasePrefab, spawnPosition, Quaternion.identity);
        newPopup.Initialize(popupColor);

        ++_spawnedPopups;

        // Keep track of this popup util it is destroyed
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
            Camera cam = GameManager.Instance.MainCamera;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = cam.transform.position.y - _spawnPositionY;
            spawnPos = cam.ScreenToWorldPoint(mousePos);
            spawnPos.y = _spawnPositionY;
        }
        else
        {
            // Find a position inside the bounds of the game area
            float areaWidth = GameManager.Instance.GameAreaWidth;
            float areaHeight = GameManager.Instance.GameAreaHeight;
            float posX = Random.Range(-areaWidth, areaWidth);
            float posZ = Random.Range(-areaHeight, areaHeight);
            spawnPos = new Vector3(posX, _spawnPositionY, posZ);
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