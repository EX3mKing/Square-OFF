using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorHealth : Damageable
{
    public PlayerEnergy playerEnergy;
    
    public override float TakeDamage(Element element,float damage)
    {
        playerEnergy.TakeDamage(element, damage);
        return damage;
    }
}
