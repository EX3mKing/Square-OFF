using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Damageable
{
    public float health = 100;
    public GameObject drop;

    public W_Enemy weapon;
    public LayerMask shootAtLayer;
    public float distanceToAttack = 10;
    public float distanceToFollow = 20;
    public float timeToFollow = 0.4f;
    public float timeToAttack = 0.2f;
    public float timeToLoseAggro = 0.2f;
    
    private NavMeshAgent _agent;
    private Transform _player;
    
    private float _currentTimeToFollow;
    private float _currentTimeToAttack;
    private float _currentTimeToLoseAggro;
    
    public virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameManager.instance.player.transform;
        _currentTimeToAttack = timeToAttack;
        _currentTimeToFollow = timeToFollow;
    }

    public virtual void Update()
    {
        weapon.isAiming = false;
        RaycastHit raycast;
        Physics.Raycast(transform.position, _player.position - transform.position, out raycast, 
            distanceToFollow, shootAtLayer);

        if (raycast.collider == null || !raycast.collider.CompareTag("Player"))
        {
            _currentTimeToLoseAggro -= Time.deltaTime;
        }
        
        if (_currentTimeToLoseAggro <= 0)
        {
            _currentTimeToFollow = timeToFollow;
            _currentTimeToAttack = timeToAttack;
            _currentTimeToLoseAggro = timeToLoseAggro;
            _agent.SetDestination(transform.position);
            return;
        }

        _currentTimeToFollow -= Time.deltaTime;

        float distance = Vector3.Distance(transform.position, _player.position);
        if (distance < distanceToAttack)
        {
            _currentTimeToAttack -= Time.deltaTime;
            if (_currentTimeToAttack <= 0)
            {
                _agent.SetDestination(transform.position);
                weapon.isAiming = true;
                weapon.ShootBullets();
            }
        }
        else
        {
            if (_currentTimeToFollow <= 0) _agent.SetDestination(_player.position);
            else _agent.SetDestination(transform.position);
        }
        
    }
    
    public override float TakeDamage(Element element, float damage)
    {
        damage = base.TakeDamage(element, damage);
        
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            Instantiate(drop, transform.position, Quaternion.identity);
        }
        
        return damage;
    }
}
