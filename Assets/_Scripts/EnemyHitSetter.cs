using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IEnemyHitSetter
{
    public void Hit(ObjectPool<Bullet> bulletPool, Vector2 position, EnemyHitData hitData);
}

public class RedHitSetter : IEnemyHitSetter
{
    public void Hit(ObjectPool<Bullet> bulletPool, Vector2 position, EnemyHitData hitData) {}
}
public class YellowHitSetter : IEnemyHitSetter
{
    public void Hit(ObjectPool<Bullet> bulletPool, Vector2 position, EnemyHitData hitData)
    {
        int n = Random.Range(2, 10);
        for (int i = 0; i < n; i++)
        {
            float angle = 2 * Mathf.PI * i / n;
            Vector2 shotDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Bullet bullet = bulletPool.Get();
            bullet.StateUpdate(shotDirection, position, hitData._bulletSpeed, hitData._bulletDamage);
        }
    }
}
