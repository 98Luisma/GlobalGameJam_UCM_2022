using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Boss Enemy")]
    [SerializeField] private float _moveToCenterSeconds = 3f;
    [SerializeField] private float _swayPeriod = 5f;
    [SerializeField] private float _swayAmplitude = 10f;
    [SerializeField] private Vector3 _targetPosition = new Vector3(0f, 0f, 7f);

    private bool _hasReachedCenter = false;
    private float _swayTimer = 0f;

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
        transform.position = Vector3.Lerp(_initPosition, _targetPosition, _timeSinceCreation / _moveToCenterSeconds);

        // Change state?
        if (_timeSinceCreation >= _moveToCenterSeconds)
        {
            _hasReachedCenter = true;
            StartShooting();
        }
    }

    private void MoveInPlace(float dt)
    {
        _swayTimer += dt;
        float angle = 2f * Mathf.PI * _swayTimer / _swayPeriod;
        float newX = _swayAmplitude * Mathf.Sin(angle);
        transform.position = new Vector3(newX, 0f, transform.position.z);
    }
}