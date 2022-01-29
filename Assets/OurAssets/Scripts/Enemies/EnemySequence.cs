using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Sequence of enemies to be spawned.
///</summary>
[CreateAssetMenu(menuName = "ScriptableObjects/EnemySequence", fileName = "New EnemySequence")]
public class EnemySequence : ScriptableObject
{
    [SerializeField] private Enemy _enemyPrefab = null;
    [SerializeField] private int _amount = 3;
    [SerializeField] private float _delaySeconds = 1f;
    [SerializeField] private Vector2 _initialDirection = new Vector2(0f, 1f);
    [SerializeField, Range(0, 1f)] private float _normalizedSpawnPosX = 0.5f;
    [SerializeField, Range(0, 1f)] private float _normalizedSpawnPosZ = 1f;

#region Getters
    public Enemy EnemyPrefab { get => _enemyPrefab; }
    public int Amount { get => _amount; }
    public float DelaySeconds { get => _delaySeconds; }
    public Vector2 InitialDirection { get => _initialDirection; }
    public float NormalizedSpawnPosX { get => _normalizedSpawnPosX; }
    public float NormalizedSpawnPosZ  { get => _normalizedSpawnPosZ; }
#endregion
}