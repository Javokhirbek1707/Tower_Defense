using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingAttackTower : MonoBehaviour
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
    private float _damage = 10f;

    [SerializeField]
    private float _fireRate = 0.5f;

    [SerializeField]
    private AudioClip _fireSound;

    private AudioSource _audioSource;

    private bool _attackCD = false;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _audioSource.clip = _fireSound;
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
    }

    void Update()
    {
        TowerHealth health = GetComponentInParent<TowerHealth>();
        if (health == null || health.isPlaced == false)
        {
            return;
        }

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

            if (_audioSource.isPlaying == false)
            {
                _audioSource.Play();
            }

            if (_attackCD == false)
            {
                _attackCD = true;
                _mainTarget.GetComponent<EnemyAI>().TakeDamage(_damage);
                StartCoroutine(AttackCooldown());
            }
        }
        else
        {
            if (_audioSource.isPlaying == true)
            {
                _audioSource.Stop();
            }
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