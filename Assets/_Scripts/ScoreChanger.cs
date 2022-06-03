using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreChanger : MonoBehaviour
{
    [SerializeField] private ScoreCounter _scoreConter;
    private Text _text;
    
    private void Awake()
    {
        _text = GetComponent<Text>();
    }
    private void Start()
    {
        _scoreConter.OnScoreChanged.AddListener(ChangeScoreUI);
    }
    private void ChangeScoreUI(int score)
    {
        _text.text = "Score: " + score;
    }
}
