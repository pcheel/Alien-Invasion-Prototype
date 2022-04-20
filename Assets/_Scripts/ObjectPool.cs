using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _poolSize;
    private List<Enemy> _enemyPool;

    private void Awake()
    {
        _enemyPool = new List<Enemy>();
        PoolInstantiate(_poolSize);
    }
    private void PoolInstantiate(float poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            AddObjectToPool();
        }
    }
    private void AddObjectToPool()
    {
        GameObject enemyPrefab = Instantiate(_enemyPrefab);
        _enemyPool.Add(enemyPrefab.GetComponent<Enemy>());
        enemyPrefab.transform.SetParent(this.transform);
        enemyPrefab.GetComponent<Enemy>().pool = this;      //?
        enemyPrefab.SetActive(false);
    }

    public void ReturnInstance(GameObject _object)
    {
        _object.SetActive(false);
    }
    public GameObject GetObject()
    {
        for (int i = 0; i < _enemyPool.Count; i++)
        {
            if (_enemyPool[i].gameObject.activeInHierarchy == false)
            {
                _enemyPool[i].gameObject.SetActive(true);
                return _enemyPool[i].gameObject;
            }
        }
        AddObjectToPool();
        _enemyPool[_enemyPool.Count - 1].gameObject.SetActive(true);
        return _enemyPool[_enemyPool.Count - 1].gameObject;
    }
}
