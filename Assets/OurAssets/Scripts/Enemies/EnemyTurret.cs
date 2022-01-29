using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _period = 1f;
    [Header("Visuals")]
    [SerializeField] private bool _lockOnPlayer = true;
    [SerializeField] private Transform _meshTransform = null;

    private Transform _playerTransform;
    private bool _shouldShoot = false;
    private float _shootTimer = 0f;

    private void Start()
    {
        _playerTransform = GameManager.Instance.Player.transform;
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
        if (_lockOnPlayer)
        {
            _meshTransform.LookAt(_playerTransform, Vector3.up);
        }
    }

    private void ShootBullet()
    {

    }

    public void StartShooting() => _shouldShoot = true;

    public void StopShooting() => _shouldShoot = false;
}
