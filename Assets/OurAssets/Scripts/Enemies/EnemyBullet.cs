using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Bullet shot by enemies.
///</summary>
[RequireComponent(typeof(SphereCollider))]
public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _maxLifetime = 10f;
    [SerializeField] private ParticleSystem _explosionParticles = null;

    private Vector3 _moveDir;

    private void Start()
    {
        Destroy(gameObject, _maxLifetime);
    }

    private void Update()
    {
        transform.Translate(_moveDir * _speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HitPlayer();
        }
    }

    public void Shoot(Vector3 direction)
    {
        _moveDir = direction;
    }

    private void HitPlayer()
    {
        GameManager.Instance.addLife(-_damage);

        Instantiate(_explosionParticles, transform.position, Quaternion.identity);

        // TODO: Play a sound

        Destroy(gameObject);
    }
}