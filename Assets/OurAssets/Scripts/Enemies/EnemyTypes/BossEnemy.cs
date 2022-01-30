using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Boss Enemy")]
    [SerializeField] private float _moveToCenterSeconds = 10f;
    [SerializeField] private float _startShootingSeconds = 7f;
    [SerializeField] private float _swayPeriod = 5f;
    [SerializeField] private float _swayAmplitude = 10f;
    [SerializeField] private Vector3 _targetPosition = new Vector3(0f, 0f, 7f);
    [Header("Visuals")]
    [SerializeField] private Vector3 _rotation = new Vector3(0f, -108.57f, 0f);

    private bool _hasReachedCenter = false;
    private bool _hasStartedShooting = false;
    private float _swayTimer = 0f;

    private void Start()
    {
        transform.eulerAngles = _rotation;
        GameManager.Instance.OnBossSpawned();
    }

    protected override void StartEnemyLogic()
    {
        StopShooting();
    }

    protected override void MoveEnemy(float dt)
    {
        if (!_hasReachedCenter)
        {
            MoveTowardsCenter(dt);
        }
        else
        {
            MoveInPlace(dt);
        }
    }

    private void MoveTowardsCenter(float dt)
    {
        float x = _timeSinceCreation / _moveToCenterSeconds;
        float t = 1f - Mathf.Pow(1f-x, 3f);
        transform.position = Vector3.Lerp(
            _initPosition,
            _targetPosition + Vector3.right * _swayAmplitude,
            t
        );

        // Start shooting?
        if (!_hasStartedShooting && _timeSinceCreation >= _startShootingSeconds)
        {
            StartShooting();
            _hasStartedShooting = true;
        }

        // Change state?
        if (_timeSinceCreation >= _moveToCenterSeconds)
        {
            _hasReachedCenter = true;
        }
    }

    private void MoveInPlace(float dt)
    {
        _swayTimer += dt;
        float angle = 2f * Mathf.PI * _swayTimer / _swayPeriod;
        float newX = _swayAmplitude * Mathf.Cos(angle);
        transform.position = new Vector3(newX, 0f, transform.position.z);
    }

    protected override void EnemyDeath()
    {
        GameManager.Instance.OnBossDefeated();
    }
}