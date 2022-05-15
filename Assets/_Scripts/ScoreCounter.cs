using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    private Text _text;
    private int _score = 0;
    
    private void Awake()
    {
        _text = GetComponent<Text>();
        EventManager.OnScoreChanged.AddListener(ChangeScore);
    }
    private void ChangeScore(int score)
    {
        _score += score;
        _text.text = "Score: " + _score;
    }
}
