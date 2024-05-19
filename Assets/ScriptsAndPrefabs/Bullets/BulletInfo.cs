using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BI_", menuName = "BulletInfo", order = 0)]
public class BulletInfo : ScriptableObject
{
    public int bulletsInShell;
    public float energyCost;
    public float damage;
    public float criticalDamage;
    public float speed;
    public float inAccuracyX;
    public float inAccuracyY;
    public float lifeTime;
    public float gravity;
}
