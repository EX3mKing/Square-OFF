using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "U_", menuName = "Upgrade", order = 0)]
public class Upgrade : ScriptableObject
{
    public Element element;
    
    [Header("Rifle Stats")]
    public int shellsPerShot;
    public int shotsPerBurst;
    public float fireRate;
    public float accuracy;
    public float timeBetweenBurstShots;

    [Header("Bullet Stats")]
    public int bulletsInShell;
    public float energyCost;
    public float damage;
    public float criticalDamage;
    public float speed;
    public float shellAccuracy;
    public float gravity;
}
