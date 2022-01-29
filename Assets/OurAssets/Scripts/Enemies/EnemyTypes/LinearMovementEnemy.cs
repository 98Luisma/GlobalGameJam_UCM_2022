using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// An enemy that enters the screen from one side and exits through naother.
///</summary>
public class LinearMovementEnemy : Enemy
{
    [Header("LinearMovementEnemy")]
    [SerializeField] private float _speed = 2f;

    protected override void MoveEnemy(float dt)
    {
        Vector3 moveDir = _initDirection * _speed * dt;
        transform.Translate(moveDir, Space.World);
    }
}
