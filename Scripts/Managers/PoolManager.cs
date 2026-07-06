using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Manager : MonoBehaviour
{
    private static Pool_Manager _instance;
    public static Pool_Manager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Pool Manager is NULL");
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    [SerializeField]
    private int _mech1PoolSize = 10;

    [SerializeField]
    private int _mech2PoolSize = 10;

    [SerializeField]
    private List<GameObject> _mech1Pool = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _mech2Pool = new List<GameObject>();

    [SerializeField]
    private GameObject _aiContainer;

    [SerializeField]
    private GameObject _mech1Prefab;

    [SerializeField]
    private GameObject _mech2Prefab;

    void Start()
    {
        GenerateMech1(_mech1PoolSize);
        GenerateMech2(_mech2PoolSize);
    }

    private List<GameObject> GenerateMech1(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = Instantiate(_mech1Prefab);
            enemy.transform.parent = _aiContainer.transform;
            enemy.SetActive(false);
            _mech1Pool.Add(enemy);
        }
        return _mech1Pool;
    }

    private List<GameObject> GenerateMech2(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = Instantiate(_mech2Prefab);
            enemy.transform.parent = _aiContainer.transform;
            enemy.SetActive(false);
            _mech2Pool.Add(enemy);
        }
        return _mech2Pool;
    }

    public GameObject RequestMech1()
    {
        foreach (var enemy in _mech1Pool)
        {
            if (enemy.activeInHierarchy == false)
            {
                return enemy;
            }
        }
        GameObject newEnemy = Instantiate(_mech1Prefab);
        newEnemy.transform.parent = _aiContainer.transform;
        _mech1Pool.Add(newEnemy);
        return newEnemy;
    }

    public GameObject RequestMech2()
    {
        foreach (var enemy in _mech2Pool)
        {
            if (enemy.activeInHierarchy == false)
            {
                return enemy;
            }
        }
        GameObject newEnemy = Instantiate(_mech2Prefab);
        newEnemy.transform.parent = _aiContainer.transform;
        _mech2Pool.Add(newEnemy);
        return newEnemy;
    }
}