using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyComet : Damageable
{
    public CometElement cometElement;
    public List<GameObject> enemies;
    public float fallSpeed;
    
    public Vector3 enemySpawnPoint;
    public Vector3 target;
    public float spawnRate = 1;
    public GameObject impactParticle;
    public ParticleSystem trailParticle;
    
    public Collider cometCollider;
    public float cometHealth;
    
    private int _curEnemySpawnIndex;
    
    public void SpawnComet(Vector3 cometTarget, Vector3 spawnTarget)
    {
        cometCollider.enabled = false;
        target = cometTarget;
        enemySpawnPoint = spawnTarget;
        transform.LookAt(target);
        StartCoroutine(MoveToGround());
    }
    
    private IEnumerator MoveToGround()
    {
        float fallTime = Vector3.Distance(transform.position, target) / fallSpeed;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / fallTime;
            transform.position += transform.forward.normalized * (fallSpeed * Time.deltaTime);
            yield return 0;
        }
        
        trailParticle.Stop();
        cometCollider.enabled = true;
        Instantiate(impactParticle, transform.position, Quaternion.identity);
        InvokeRepeating("SpawnEnemies", 0, spawnRate);
    }

    private void SpawnEnemies()
    {
        Instantiate(enemies[_curEnemySpawnIndex], enemySpawnPoint, Quaternion.identity);
        _curEnemySpawnIndex++;
        if (_curEnemySpawnIndex == enemies.Count) CancelInvoke("SpawnEnemies");
    }
    
    public override float TakeDamage(Element element, float damage)
    {
        damage = base.TakeDamage(element, damage);
        cometHealth -= damage;
        
        if (cometHealth <= 0)
        {
            UpgradeManger.instance.CometMined(cometElement);
            Destroy(gameObject);
        }
        
        return damage;
    }
}
