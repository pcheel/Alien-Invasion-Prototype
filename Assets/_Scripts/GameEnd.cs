using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour
{
    [SerializeField] Player _player;

    private void Start()
    {
        _player.OnPlayerDied.AddListener(EndGame);
    }
    private void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
