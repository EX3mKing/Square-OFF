using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Base : Weapon
{
    public override void ShootBullets()
    {
        if (shootCooldownTimer > 0) return; // if the cooldown is not 0, return
        shootCooldownTimer = shootCooldown; // reset the timer of the cooldown
        
        float cost = GameManager.instance.playerEnergy.UseEnergy(element, bulletScript.energyCost);
        if (cost < 0) return;
        
        for (int k = 0; k < shotsPerBurst; k++)
        {
            Invoke("SpawnBullets", k * timeBetweenBurstShots);
        }
    }
}
