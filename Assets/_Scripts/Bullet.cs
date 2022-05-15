using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private int _damage;
    private float _speed;
    private Vector2 _position;
    private Vector2 _direction;
    private ObjectPool<GameObject> _pool;
    private const float LIMIT_POSITION = 40F;

    public int Damage
    {
        get { return _damage; }
    }
    public ObjectPool<GameObject> BulletPool  //???
    {
        set { _pool = value; }
        get { return _pool; }
    }
    public void StateUpdate(Vector2 direction, Vector2 position, float speed, int damage)
    {
        _speed = speed;
        _direction = direction;
        _position = position;
        _damage = damage;
    }

    private void Update()
    {
        if (Mathf.Abs(_position.x) > LIMIT_POSITION || Mathf.Abs(_position.y) > LIMIT_POSITION)
        {
            _pool.Release(gameObject);
            return;
        }
        Move();
    }
    private void Move()
    {
        _position += _direction * _speed * Time.deltaTime;
        transform.position = _position;
    }
}
