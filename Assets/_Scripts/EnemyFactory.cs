using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyFactory
{
    public abstract EnemyMovement CreateMovement();
    public abstract EnemyHit CreateHit();
}
public abstract class EnemyMovement : MonoBehaviour
{
    public abstract void Move(Transform transform, float speed);
}
public abstract class EnemyHit : MonoBehaviour
{
    public abstract void Hit(int damage);
}


public class CubeFactory : EnemyFactory
{
    public override EnemyMovement CreateMovement()
    {
        return new CubeMovement();
    }
    public override EnemyHit CreateHit()
    {
        return new CubeHit();
    }
}
public class CubeMovement : EnemyMovement
{
    public override void Move(Transform transform, float speed)
    {
        Vector3 direction = Player._player.Position - transform.position;
        direction = Vector3.ClampMagnitude(direction, 1f);
        transform.position += direction * Time.deltaTime * speed;
    }
}
public class CubeHit : EnemyHit
{
    public override void Hit(int damage)
    {
        Player._player.ApplyDamage(damage);
    }
}
