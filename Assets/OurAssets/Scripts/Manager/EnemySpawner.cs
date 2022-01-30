using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private float _spawnPositionY = 0f;
    [SerializeField] private float _spawnSequencePeriod = 8f;
    [SerializeField] private float _extraOffset = 3f;
    [Header("Enemy references")]
    [SerializeField] private EnemyBullet _enemyBulletPrefab;
    [SerializeField] private EnemySequence[] _enemySequences;
    [SerializeField] private EnemySequence _bossSequence;
    [SerializeField] private float _extraBossOffsetZ = 5f;
    [Header("Difficulty")]
    [SerializeField] private int _secondsPerDifficultyLevel = 15;
    [SerializeField] private int _maxEnemiesPerSequence = 7;
    [SerializeField] private float _reducedWaitPerDifficultyLevel = 0.5f;
    [SerializeField] private float _minWaitBetweenSequences = 4f;
    [SerializeField] private int _sequencesBetweenBossfights = 10;
    [Header("Debug")]
    [SerializeField] private bool DEBUG_START_WITH_BOSS_BATTLE = false;

    private bool _shouldSpawn = false;
    private float _spawnTimer = 0f;
    private Vector3[] _screenCorners = new Vector3[4];
    private ObjectPool<EnemyBullet> _enemyBulletPool;
    private float _elapsedTime = 0f;
    private int _difficultyLevel = 0;
    private float _timeUntilNextSpawn;
    private int _lastDifficultyLevel = 0;

    private int _sequencesSinceLastBossFight = 0;
    private bool _isInBossBattle = false;
    private Enemy _instantiatedBoss = null;

    private void Awake()
    {
        _enemyBulletPool = new ObjectPool<EnemyBullet>(_enemyBulletPrefab, 60);
        _timeUntilNextSpawn = _spawnSequencePeriod;
    }

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

        if (DEBUG_START_WITH_BOSS_BATTLE)
        {
            StartBossBattle();
        }
    }

    private void Update()
    {
        // Early returns
        if (!_shouldSpawn) return;
        if (_isInBossBattle) return;

        // Calculate the difficulty
        _elapsedTime += Time.deltaTime;
        _difficultyLevel = (int)(_elapsedTime / _secondsPerDifficultyLevel);
        if (_difficultyLevel > _lastDifficultyLevel)
        {
            _lastDifficultyLevel = _difficultyLevel;
            GameManager.Instance.OnDifficultyIncreased();
        }
        
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= _timeUntilNextSpawn)
        {
            if (_sequencesSinceLastBossFight >= _sequencesBetweenBossfights)
            {
                StartBossBattle();
                _sequencesSinceLastBossFight = 0;
            }
            else
            {
                // Spawn a random sequence
                EnemySequence sequence = _enemySequences[Random.Range(0, _enemySequences.Length)];
                StartCoroutine(SpawnEnemySequence(sequence));
                
                // Prepare the next spawn sequence
                _spawnTimer = 0f;
                _timeUntilNextSpawn = sequence.Amount * sequence.DelaySeconds + Mathf.Max(
                    _minWaitBetweenSequences,
                    _spawnSequencePeriod - _difficultyLevel * _reducedWaitPerDifficultyLevel
                );
            }
        }
    }

    ///<summary>
    /// Instantiates the enemies from a new sequence.
    ///</summary>
    private IEnumerator SpawnEnemySequence(EnemySequence sequence)
    {
        ++_sequencesSinceLastBossFight;

        // All enemies will spawn at the same point
        Vector3 spawnPos = GetSpawnPos(sequence.NormalizedSpawnPosX, sequence.NormalizedSpawnPosZ);

        // Difficulty level based on time
        int spawnAmount = Mathf.Clamp(sequence.Amount + Random.Range(-2, _difficultyLevel), 1, _maxEnemiesPerSequence);

        // Spawn the enemies one by one with a delay
        for (int i = 0; i < spawnAmount; ++i)
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

    public EnemyBullet RequestEnemyBullet() => _enemyBulletPool.RequestObject();

    public void StartBossBattle()
    {
        _isInBossBattle = true;

        // Spawn the boss using its sequence
        Vector3 spawnPos = GetSpawnPos(_bossSequence.NormalizedSpawnPosX, _bossSequence.NormalizedSpawnPosZ);
        spawnPos += Vector3.forward * _extraBossOffsetZ;
        _instantiatedBoss = Instantiate(_bossSequence.EnemyPrefab, spawnPos, Quaternion.identity);

        // Initialize the enemy with the correct variables
        Vector3 initDir = new Vector3(_bossSequence.InitialDirection.x, 0f, _bossSequence.InitialDirection.y);
        _instantiatedBoss.Initialize(initDir);

        // Find the boss (probably very inneficient)
        _instantiatedBoss = FindObjectOfType<BossEnemy>();
        _instantiatedBoss.OnEnemyDestroyed += EndBossBattle;
    }

    private void EndBossBattle()
    {
        _isInBossBattle = false;
        if (_instantiatedBoss)
        {
            _instantiatedBoss.OnEnemyDestroyed -= EndBossBattle;
        }
    }
}
