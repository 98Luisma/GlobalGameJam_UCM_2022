using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private float _period = 1f;
    [SerializeField, Range(0,1)] private float _periodOffset = 0f;
    [Header("Visuals")]
    [SerializeField] private bool _lockOnPlayer = true;
    [SerializeField] private Transform _meshTransform = null;

    private Transform _playerTransform;
    private Transform _enemyTransform;
    private bool _shouldShoot = false;
    private float _shootTimer = 0f;

    private void Start()
    {
        _playerTransform = GameManager.Instance.Player.transform;
        _enemyTransform = transform.parent;

        _shootTimer = _periodOffset * _period;
    }

    private void Update()
    {
        if (_shouldShoot)
        {
            _shootTimer += Time.deltaTime;
            if (_shootTimer >= _period)
            {
                ShootBullet();
                _shootTimer = 0f;
            }
        }
    }

    private void LateUpdate()
    {
        if (_lockOnPlayer && _meshTransform)
        {
            _meshTransform.LookAt(_playerTransform, Vector3.up);
        }
    }

    private void ShootBullet()
    {
        Vector3 shootDir = _lockOnPlayer
            ? Vector3.Normalize(_playerTransform.position - _enemyTransform.position)
            : _enemyTransform.forward;
    
        EnemyBullet newBullet = GameManager.Instance.RequestEnemyBullet();
        newBullet.transform.position = new Vector3(
            transform.position.x,
            _enemyTransform.position.y,
            transform.position.z
        );
        newBullet.Shoot(shootDir);

        GameManager.Instance.OnEnemyBulletSpawned();
    }

    public void StartShooting() => _shouldShoot = true;

    public void StopShooting() => _shouldShoot = false;
}
