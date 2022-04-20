using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEnemy : Enemy
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;

    private void Awake()
    {
        _factory = new CubeFactory();
        _movement = _factory.CreateMovement();
        _hit = _factory.CreateHit();
    }

    private void Update()
    {
        _movement.Move(transform, _speed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            _hit.Hit(_damage);
            Die();
        }
    }
}
