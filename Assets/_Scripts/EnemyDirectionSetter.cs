using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyDirectionSetter
{
    public Vector2 GetDirection(Vector3 position, Vector3 playerPosition);
}

public class CubeDirectionSetter : IEnemyDirectionSetter
{
    public Vector2 GetDirection(Vector3 position, Vector3 playerPosition)
    {
        Vector3 direction = playerPosition - position;
        return direction;
    }
}
public class RhombusDirectionSetter : IEnemyDirectionSetter
{
    public Vector2 GetDirection(Vector3 position, Vector3 nothing)
    {
        return Vector3.zero;
    }
}
public class TriangleDirectionSetter : IEnemyDirectionSetter
{
    public Vector2 GetDirection(Vector3 position, Vector3 playerPosition)
    {
        Vector3 normal = playerPosition - position;
        Vector3 tangent = Vector3.Cross(normal, Vector3.forward);
        Vector3 direction = 2 * tangent + normal;
        return direction;
    }
}