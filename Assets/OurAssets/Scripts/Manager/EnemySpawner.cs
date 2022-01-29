using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnPositionY = 0f;
    [SerializeField] private float _spawnSequencePeriod = 5f;
    [SerializeField] private float _extraOffset = 3f;
    [Space]
    [SerializeField] private EnemySequence[] _enemySequences;

    private bool _shouldSpawn = false;
    private float _spawnTimer = 0f;
    private Vector3[] _screenCorners = new Vector3[4];

    private void Start()
    {
        // Save the corners for later use
        Camera mainCamera = GameManager.Instance.MainCamera;
        float zDepth = mainCamera.transform.position.y - _spawnPositionY;
        Rect rect = new Rect(0f, 0f, 1f, 1f);
        mainCamera.CalculateFrustumCorners( rect, zDepth, Camera.MonoOrStereoscopicEye.Mono, _screenCorners);
        for (int i = 0; i < 4; ++i)
        {
            _screenCorners[i] = mainCamera.transform.TransformPoint(_screenCorners[i]);
        }
    }

    private void Update()
    {
        if (!_shouldSpawn) return;
        
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _spawnSequencePeriod)
        {
            StartCoroutine(SpawnEnemySequence());
            _spawnTimer = 0f;
        }
    }

    ///<summary>
    /// Instantiates the enemies from a new sequence.
    ///</summary>
    private IEnumerator SpawnEnemySequence()
    {
        // Get a random sequence to spawn
        EnemySequence sequence = _enemySequences[Random.Range(0, _enemySequences.Length)];

        // All enemies will spawn with the same properties
        Vector3 spawnPos = GetSpawnPos(sequence.NormalizedSpawnPosX, sequence.NormalizedSpawnPosZ);

        // Spawn the enemies one by one with a delay
        for (int i = 0; i < sequence.Amount; ++i)
        {
            Enemy newEnemy = Instantiate(sequence.EnemyPrefab, spawnPos, Quaternion.identity);

            // Initialize the enemy with the correct variables
            Vector3 initDir = new Vector3(sequence.InitialDirection.x, 0f, sequence.InitialDirection.y);
            newEnemy.Initialize(initDir);

            yield return new WaitForSeconds(sequence.DelaySeconds);
        }
    }

    ///<summary>
    /// Returns a valid spawn position near the edge of the camera view.
    ///</summary>
    private Vector3 GetSpawnPos(float normalizedX, float normalizedZ)
    {
        // Get a point in the border of the camera view
        float posX = Mathf.Lerp(_screenCorners[0].x, _screenCorners[3].x, normalizedX);
        float posZ = Mathf.Lerp(_screenCorners[0].z, _screenCorners[1].z, normalizedZ);
        Vector3 spawnPos = new Vector3(posX, _spawnPositionY, posZ);

        // Add an extra offset in the correct direction
        Vector3 cameraPos = GameManager.Instance.MainCamera.transform.position;
        Vector3 offsetDir = Vector3.Normalize(spawnPos - cameraPos);
        offsetDir.y = 0;
        spawnPos += offsetDir * _extraOffset;

        return spawnPos;
    }

    public void SetShouldSpawn(bool value) => _shouldSpawn = value;
}
