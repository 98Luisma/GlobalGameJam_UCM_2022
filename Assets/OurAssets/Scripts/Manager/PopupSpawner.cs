using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private float _spawnWait = 5f;
    [SerializeField] private Popup[] _popupPrefabs;
    [SerializeField] private Color[] _popupColors;
    [Header("Spawn")]
    [SerializeField] private float _spawnPositionY = 5f;
    [SerializeField] private int _maxSimultaneousPopups = 10;
    [SerializeField, Range(0, 1)] private float _spawnOnCursorProbability = 0.2f;
    [SerializeField] private int _maxSpawnAttempts = 5;
    [Header("Difficulty")]
    [SerializeField] private int _secondsPerDifficultyLevel = 15;
    [SerializeField] private float _reducedWaitPerDifficultyLevel = 0.5f;
    [SerializeField] private float _minWaitBetweenPopups = 2f;

    private bool _shouldSpawn = false;
    private float _spawnTimer = 0f;
    private int _spawnedPopups = 0;
    private bool _spawnedAtLeastOne = false;

    private float _elapsedTime = 0f;
    private int _difficultyLevel = 0;
    private float _timeUntilNextSpawn;

    private void Awake()
    {
        _timeUntilNextSpawn = _spawnWait;
    }

    private void Update()
    {
        // Early returns
        if (!_shouldSpawn) return;
        if (_spawnedPopups >= _maxSimultaneousPopups) return;

        // Calculate the difficulty
        _elapsedTime += Time.deltaTime;
        _difficultyLevel = (int)(_elapsedTime / _secondsPerDifficultyLevel);

        // Spawn a popup every _spawnWait seconds
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _timeUntilNextSpawn)
        {
            SpawnPopup();

            // Prepare the next spawn
            _spawnTimer = 0f;
            _timeUntilNextSpawn = Mathf.Max(
                _minWaitBetweenPopups,
                _spawnWait - _difficultyLevel * _reducedWaitPerDifficultyLevel
            );
        }
    }

    public void SetShouldSpawn(bool value) => _shouldSpawn = value;

    ///<summary>
    /// Instantiates and intiializes a new popup.
    ///</summary>
    private void SpawnPopup()
    {
        // Select a random prefab
        Popup popupPrefab = _popupPrefabs[Random.Range(0, _popupPrefabs.Length)];

        // Select a random color
        Color popupColor = _popupColors[Random.Range(0, _popupColors.Length)];

        // Select a position
        Vector3 spawnPosition = Vector3.zero; ;
        int attempts = 0;
        while (true)
        {
            ++attempts;
            if (attempts > _maxSpawnAttempts)
            {
                // Failed to find a position.
                // Don't spawn a popup
                return;
            }

            spawnPosition = FindSpawnPosition();

            Vector3 testOffset = new Vector3(
                Mathf.Sign(spawnPosition.x) * popupPrefab.GetWidth() * 0.75f,
                0f,
                Mathf.Sign(spawnPosition.z) * popupPrefab.GetHeight() * 0.75f
            );

            // Is the popup overlapping another popup?
            Collider[] detectedColl = new Collider[1];
            Physics.OverlapBoxNonAlloc(
                spawnPosition,
                new Vector3(popupPrefab.GetWidth() * 0.75f, 0.05f, popupPrefab.GetHeight() * 0.75f),
                detectedColl,
                Quaternion.identity,
                LayerMask.GetMask("Popup")
            );
            if (detectedColl[0])
            {
                // Cannot spawn here
                continue;
            }

            // Is the popup completely inside of the screen?
            if (GameManager.Instance.MainCamera.IsPointInBounds(spawnPosition + testOffset))
            {
                // This position is valid
                break;
            }
        }

        Popup newPopup = Instantiate<Popup>(popupPrefab, spawnPosition, Quaternion.identity);
        newPopup.Initialize(popupColor);

        // Keep track of this popup util it is destroyed
        ++_spawnedPopups;
        newPopup.OnPopupDestroyed += PopupDespawned;

        if (!_spawnedAtLeastOne)
        {
            GameManager.Instance.OnFirstPopupOpen();
            _spawnedAtLeastOne = true;
        }
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
        // spawnPos.y = _spawnPositionY;

        return spawnPos;
    }

    ///<summary>
    /// Called when a Popup despawns.
    ///</summary>
    private void PopupDespawned(Popup popup)
    {
        --_spawnedPopups;
        _spawnedPopups = Mathf.Max(_spawnedPopups, 0);
        popup.OnPopupDestroyed -= PopupDespawned;

        if (_spawnedPopups == 0)
        {
            GameManager.Instance.OnAllPopupsDespawned();
        }
    }
}