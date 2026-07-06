using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDamage : MonoBehaviour
{
    private GameObject _target;
    private float _damage;
    private bool _hasHit = false;

    public void SetTarget(GameObject target, float damage)
    {
        _target = target;
        _damage = damage;
    }

    void Update()
    {
        if (_target == null || _hasHit == true)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _target.transform.position);

        if (distance < 1.5f)
        {
            _hasHit = true;
            EnemyAI enemy = _target.GetComponent<EnemyAI>();

            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
            }
        }
    }
}
