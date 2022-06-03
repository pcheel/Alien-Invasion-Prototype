using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyHitData", menuName = "Data/EnemyHit", order = 51)]
public class EnemyHitData : ScriptableObject
{
    public int _collisionDamage;
    public int _hitScore;
    public float _shotDelay;
    public BulletData _bulletData;
}
