using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreCounter : MonoBehaviour
{
    private int _score = 0;

    public UnityEvent<int> OnScoreChanged = new UnityEvent<int>();

    public void ChangeScore(int score)
    {
        _score += score;
        OnScoreChanged?.Invoke(_score);
    }
}
