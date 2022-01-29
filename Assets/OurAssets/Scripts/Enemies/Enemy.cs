using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 2;
    [SerializeField] private int _pointsAwardedOnDeath = 1;
    [SerializeField] private int _moneyAwardedOnDeath = 100;
    [SerializeField] EnemyTurret[] _turrets;
    [SerializeField] private ParticleSystem _deathParticles = null;
    [SerializeField] private bool _lookTowardsMoveDirection = false;

    private int _currentHealth;
    private bool _hasEnteredView = false;
    protected Vector3 _initDirection = Vector3.right;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    private void Update()
    {
        // Check if it is in/out of camera view
        if (GameManager.Instance.MainCamera.IsPointInBounds(transform.position))
        {
            if (!_hasEnteredView)
            {
                _hasEnteredView = true;
                OnEnterScreen();
            }
        }
        else
        {
            if (_hasEnteredView)
            {
                _hasEnteredView = false;
                OnExitScreen();
            }
        }

        // This is overwritten by child classes
        MoveEnemy(Time.deltaTime);
    }

    public void Initialize(Vector3 initDirection)
    {
        _initDirection = initDirection;
    }

    ///<summary>
    /// Damages the enemy.
    ///</summary>
    public void Damage()
    {
        --_currentHealth;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    ///<summary>
    /// Instantiates particles and destroys this enemy.
    ///</summary>
    private void Die()
    {
        // Add points to the manager
        GameManager.Instance.AddScore(_pointsAwardedOnDeath);
        GameManager.Instance.AddMoney(_moneyAwardedOnDeath);

        if (_deathParticles)
        {
            Instantiate(_deathParticles, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }

    private void StartShooting()
    {
        foreach (EnemyTurret turret in _turrets)
        {
            turret.StartShooting();
        }
    }

    private void StopShooting()
    {
        foreach (EnemyTurret turret in _turrets)
        {
            turret.StopShooting();
        }
    }

    private void OnEnterScreen()
    {
        StartShooting();
    }

    private void OnExitScreen()
    {
        StopShooting();
        Destroy(gameObject, 1f);
    }

    ///<summary>
    /// Called on Update. Write the enemy trajectory here.
    ///</summary>
    protected abstract void MoveEnemy(float dt);
}