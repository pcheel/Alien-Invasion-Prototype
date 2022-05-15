using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    private void Awake()
    {
        EventManager.OnGameEnded.AddListener(EndGame);
    }
    private void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
