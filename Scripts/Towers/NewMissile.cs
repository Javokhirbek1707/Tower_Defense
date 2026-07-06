using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMissile : MonoBehaviour
{
    [SerializeField]
    private float _missileSpeed = 10f;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private float _explosionLifetime = 2f;

    [SerializeField]
    private float _damage = 40f;

    private Transform _target;

    public void SetTarget(Transform target, float damage)
    {
        _target = target;
        _damage = damage;
    }

    void Update()
    {
        if (_target == null)
        {
            transform.Translate(Vector3.forward * _missileSpeed * Time.deltaTime);
            return;
        }

        Vector3 direction = (_target.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        transform.position += direction * _missileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();

            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
            }

            if (_explosionPrefab != null)
            {
                GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, _explosionLifetime);
            }

            Destroy(gameObject);
        }
    }
}
