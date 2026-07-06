using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("SpawnManager is NULL");
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    [SerializeField]
    private Transform _spawnPoint;

    [SerializeField]
    private int _waveNum = 1;
    [SerializeField]
    private int _numOfMech1, _numOfMech2;
    [SerializeField]
    private int _mech1Limit, _mech2Limit;
    [SerializeField] 
    private int _mech1Spawned, _mech2Spawned;
    public int spawnedEnemies;

    private bool _mech1Done, _mech2Done;

    void Start()
    {
        UIManager.Instance.UpdateWave(_waveNum);
        NewWave();
    }

    private void NewWave()
    {
        spawnedEnemies = 0;
        _mech1Spawned = 0;
        _mech2Spawned = 0;
        _mech1Done = false;
        _mech2Done = false;

        if (_waveNum > 10)
        {
            UIManager.Instance.LevelComplete();
            return;
        }

        if (_waveNum < 2)
        {
            _numOfMech1 = 10;
            _numOfMech2 = 0;
        }
        else
        {
            _numOfMech1 += 5;
            _numOfMech2 += 3;
        }

        _mech1Limit = _numOfMech1;
        _mech2Limit = _numOfMech2;

        StartCoroutine(SpawnMech1());
        StartCoroutine(SpawnMech2());
    }

    private void SpawnAtPoint(GameObject enemy)
    {
        var agent = enemy.GetComponent<NavMeshAgent>();

        Vector3 randomOffset = new Vector3(Random.Range(-1.5f, 1.5f), 0, Random.Range(-1.5f, 1.5f));
        Vector3 spawnPos = _spawnPoint.position + randomOffset;

        if (agent != null)
        {
            agent.enabled = false;
            enemy.transform.position = spawnPos;
            enemy.transform.rotation = _spawnPoint.rotation;
            agent.enabled = true;
            agent.Warp(spawnPos);
        }
        else
        {
            enemy.transform.position = spawnPos;
            enemy.transform.rotation = _spawnPoint.rotation;
        }
        enemy.SetActive(true);
    }

    IEnumerator SpawnMech1()
    {
        while (_mech1Spawned < _mech1Limit)
        {
            GameObject enemy = Pool_Manager.Instance.RequestMech1();
            SpawnAtPoint(enemy);
            _mech1Spawned++;
            spawnedEnemies++;
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
        _mech1Done = true;
        CheckWaveComplete();
    }

    IEnumerator SpawnMech2()
    {
        while (_mech2Spawned < _mech2Limit)
        {
            GameObject enemy = Pool_Manager.Instance.RequestMech2();
            SpawnAtPoint(enemy);
            _mech2Spawned++;
            spawnedEnemies++;
            yield return new WaitForSeconds(Random.Range(4f, 6f));
        }
        _mech2Done = true;
        CheckWaveComplete();
    }

    private void CheckWaveComplete()
    {
        if (_mech1Done == true && _mech2Done == true)
        {
            StartCoroutine(NextWaveDelay());
        }
    }

    IEnumerator NextWaveDelay()
    {
        yield return new WaitForSeconds(5f);
        _waveNum++;
        UIManager.Instance.UpdateWave(_waveNum);
        NewWave();
    }
}