using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public float cyanWeakness = 1;
    public float magentaWeakness = 1;
    public float yellowWeakness = 1;
    
    public virtual float TakeDamage(Weapon.Element element, float damage)
    {
        switch (element)
        {
            case Weapon.Element.Cyan:
                damage *= cyanWeakness;
                break;
            case Weapon.Element.Magenta:
                damage *= magentaWeakness;
                break;
            case Weapon.Element.Yellow:
                damage *= yellowWeakness;
                break;
        }
        return damage;
    }
}
