using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEnemy : Enemy
{
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
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Debug.Log("col");
    ////    //Player player = collision.gameObject.GetComponent<Player>();
    ////    //Bullet bullet = collision.gameObject.GetComponent<Bullet>();
    ////    //if (collision.gameObject.GetComponent<Player>() != null)
    ////    //{
    ////    //    _hit.Hit(_damage);
    ////    //    Die();
    ////    //}
    ////    //else if (collision.gameObject.GetComponent<Bullet>() != null)
    ////    //{
    ////    //    //ApplyDamage();
    ////    //}
    //}
}
