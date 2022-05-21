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
        EventManager.OnPlayerHealthChanged.AddListener(ChangeHealthBar);
    }
    public void ChangeHealthBar(int damage)
    {
        _image.fillAmount -= (float)damage/_player.maxHealth;
    }
}
