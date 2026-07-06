using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAttackTower : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _enemies = new List<GameObject>();

    [SerializeField]
    private GameObject _mainTarget;

    [SerializeField]
    private GameObject _parent;

    [SerializeField]
    private Transform _gunRotator;

    [SerializeField]
    private GameObject _missilePrefab;

    [SerializeField]
    private Transform _missileSpawnPoint;

    [SerializeField]
    private float _damage = 20f;

    [SerializeField]
    private float _fireRate = 2f;

    private bool _attackCD = false;

    void Update()
    {
        if (_mainTarget != null && _mainTarget.activeInHierarchy == false)
        {
            _mainTarget = null;
            _enemies.RemoveAll(e => e == null || e.activeInHierarchy == false);
        }

        TargetCheck();

        if (_mainTarget != null)
        {
            Quaternion targetLook = Quaternion.LookRotation((_mainTarget.transform.position - _gunRotator.position).normalized);
            _gunRotator.rotation = targetLook;

            if (_attackCD == false)
            {
                _attackCD = true;
                FireMissile();
                StartCoroutine(AttackCooldown());
            }
        }
    }

    private void FireMissile()
    {
        if (_mainTarget == null)
        {
            return;
        }

        GameObject missile = Instantiate(_missilePrefab, _missileSpawnPoint.position, _missileSpawnPoint.rotation);
        NewMissile missileScript = missile.GetComponent<NewMissile>();

        if (missileScript != null)
        {
            missileScript.SetTarget(_mainTarget.transform, _damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemies.Add(other.gameObject);
            TargetCheck();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemies.Remove(other.gameObject);
            if (other.gameObject == _mainTarget)
            {
                _mainTarget = null;
                TargetCheck();
            }
        }
    }

    private void TargetCheck()
    {
        if (_mainTarget != null)
        {
            return;
        }

        GameObject nearestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject enemy in _enemies)
        {
            if (enemy == null || enemy.activeInHierarchy == false)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        _mainTarget = nearestEnemy;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_fireRate);
        _attackCD = false;
    }
}
