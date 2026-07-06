using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _anim;

    [SerializeField]
    private float _speed = 1.5f;

    [SerializeField]
    public float health = 100f;

    private float _healthResetRef;

    private Transform _endPoint;

    private bool _isDead = false;

    [SerializeField]
    private bool _isAttacking = false;

    private Quaternion _animatorDefaultRotation;


    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponentInChildren<Animator>();
        _animatorDefaultRotation = _anim.transform.localRotation;
        _agent.speed = _speed;
        _healthResetRef = health;
        _endPoint = GameObject.FindGameObjectWithTag("EndPoint").transform;
        _anim.ResetTrigger("Death");
        _anim.SetBool("Walk", true);
        _anim.SetBool("Attack", false);
    }

    void Update()
    {
        AnimationsMethod();

        if (health < 1 && _isDead == false)
        {
            _isDead = true;
            StartCoroutine(Death());
        }
    }

    private void AnimationsMethod()
    {
        if (_isDead == false)
        {
            if (_isAttacking == false)
            {
                _agent.isStopped = false;
                _agent.SetDestination(_endPoint.position);
                _anim.SetBool("Walk", true);
                _anim.SetBool("Attack", false);
            }
            else
            {
                _agent.isStopped = true;
                _anim.SetBool("Walk", false);
                _anim.SetBool("Attack", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndPoint"))
        {
            UIManager.Instance.LoseLife();
            Reset();
        }

        if (other.CompareTag("Turret"))
        {
            _isAttacking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            _isAttacking = false;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }

    private void Reset()
    {
        health = _healthResetRef;
        _isDead = false;
        _isAttacking = false;
        _agent.isStopped = false;
        _anim.transform.localRotation = _animatorDefaultRotation;
        _anim.Rebind();
        _anim.Update(0f);
        _anim.SetBool("Walk", true);
        _anim.SetBool("Attack", false);
        gameObject.SetActive(false);
    }

    IEnumerator Death()
    {
        _agent.isStopped = true;
        _anim.SetTrigger("Death");
        _anim.SetBool("Walk", false);
        _anim.SetBool("Attack", false);
        UIManager.Instance.AddWarFunds(150);
        yield return new WaitForSeconds(3f);
        Reset();
    }
}
