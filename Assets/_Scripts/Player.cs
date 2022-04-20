using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _maxHealth;

    private float _currentHealth;
    
    public static Player _player;

    public Vector3 Position
    {
        get { return transform.position; }
    }
    private void Awake()
    {
        _player = this;
        _currentHealth = _maxHealth;
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        float directionX = Input.GetAxis("Horizontal");
        float directionY = Input.GetAxis("Vertical");
        Vector3 direction = Vector3.ClampMagnitude(new Vector3(directionX, directionY, 0f), 1f);
        transform.position += direction * _speed * Time.deltaTime;
    }
    public void ApplyDamage(int damage)
    {
        if (_currentHealth < damage)
        {
            _currentHealth = 0;
        }
        else
        {
            _currentHealth -= damage;
        }
        Debug.Log($"Вам нанесли {damage} урона");
        Debug.Log($"_currentHealth = {_currentHealth}");
    }
}
