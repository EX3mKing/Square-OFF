using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    // accuracy is averaged between bullet's and weapon's accuracy

    public int bulletsInShell;  // number of bullets in one shell
    public float energyCost;
    public float damage;
    public float criticalDamage;
    public float speed;
    [Range(0, 100)]
    public float inAccuracyX;
    [Range(0, 100)]
    public float inAccuracyY;
    public float lifeTime;      // time before bullet is destroyed
    public float gravity;       // set to 0 for no falloff

    public GameObject hitEffect;
    public AudioClip hitSound;
    public Weapon.Type type;
    public Weapon.Element element;
    public Owner owner;
    
    private Rigidbody _rb;

    public enum Owner
    {
        Player,
        Enemy,
        Neutral,
    }

    public virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
        // add inaccuracy to the individual pellets per bullet
        Vector2 random = UnityEngine.Random.insideUnitCircle;
        transform.rotation = Quaternion.Euler(new Vector3(random.x * inAccuracyY, random.y * inAccuracyX, 0 ) 
                                              + transform.rotation.eulerAngles);
        
        // add speed to the bullet
        _rb.velocity = transform.forward * speed + Vector3.up;
        Invoke("BulletDeath", lifeTime);
    }

    public virtual void OnCollisionEnter(Collision other)
    {
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        if (damageable) damageable.TakeDamage(element, damage);
        BulletDeath();
    }
    
    public virtual void BulletDeath()
    {
        Destroy(gameObject);
    }

    public virtual void FixedUpdate()
    {
        _rb.velocity += Vector3.up * (-gravity * Time.fixedDeltaTime); // apply gravity to the bullet
    }
}
