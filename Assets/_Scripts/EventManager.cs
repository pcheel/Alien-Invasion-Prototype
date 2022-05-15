using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static readonly UnityEvent<int> OnPlayerHealthChanged = new UnityEvent<int>();
    public static readonly UnityEvent OnPlayerDamageApplied = new UnityEvent();
    public static readonly UnityEvent OnEnemyDied = new UnityEvent();
    public static readonly UnityEvent<int> OnScoreChanged = new UnityEvent<int>();
    public static readonly UnityEvent OnGameEnded = new UnityEvent();

    public static void SendPlayerDamageApplied(int damage)
    {
        OnPlayerHealthChanged?.Invoke(damage);
        OnPlayerDamageApplied?.Invoke();
    }
    public static void SendEnemyDied(int score)
    {
        OnScoreChanged?.Invoke(score);
        OnEnemyDied?.Invoke();
    }
    public static void SendGameEnded()
    {
        OnGameEnded?.Invoke();
    }
}
