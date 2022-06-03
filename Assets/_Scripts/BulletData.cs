using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "Data/Bullet", order = 51)]
public class BulletData : ScriptableObject
{
    public int _bulletDamage;
    public int _bulletSpeed;
}
