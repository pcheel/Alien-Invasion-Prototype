using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private int _maxPlayerHealth;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        EventManager.OnPlayerHealthChanged.AddListener(ChangeHealthBar);
    }
    public void ChangeHealthBar(int damage)
    {
        _image.fillAmount -= (float)damage/_maxPlayerHealth;
    }
}
