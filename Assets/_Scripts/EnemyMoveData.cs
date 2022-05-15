using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMoveData", menuName = "Data/EnemyMove", order = 51)]
public class EnemyMoveData : ScriptableObject
{
    public int _maxHealth;
    public int _moveScore;
    public float _speed;
}
