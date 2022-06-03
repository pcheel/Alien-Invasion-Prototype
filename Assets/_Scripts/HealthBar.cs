using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player _player;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    private void Start()
    {
        _player.OnPlayerHealthChanged.AddListener(ChangeHealthBar);
    }
    private void ChangeHealthBar(int currentHealth)
    {
        _image.fillAmount = (float)currentHealth/_player.maxHealth;
    }
}
