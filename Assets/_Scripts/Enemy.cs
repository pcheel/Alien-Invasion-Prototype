using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected ObjectPool _pool;
    protected EnemyFactory _factory;
    protected EnemyMovement _movement;
    protected EnemyHit _hit;
    public Action OnDied;

    protected void Die()
    {
        OnDied?.Invoke();
        _pool.ReturnInstance(this.gameObject);
    }
    public ObjectPool pool
    {
        set
        {
            _pool = value;
        }
    }
}
