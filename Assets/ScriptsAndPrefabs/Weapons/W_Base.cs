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

    public override void Aiming()
    {
        if (isAimable)
        {
            if (isAiming) // move the weapon to the middle of the screen, zoom in
            {
                transform.localPosition = Vector3.Slerp(transform.localPosition, middlePosition.localPosition, Time.deltaTime * aimSpeed); 
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, zoomFOV, Time.deltaTime * aimSpeed);
            }
            else // move the weapon back to the start position, zoom out
            {
                transform.localPosition = Vector3.Slerp(transform.localPosition, startPosition, Time.deltaTime * aimSpeed); 
                mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, GameManager.instance.defaultFOV, Time.deltaTime * aimSpeed);
            }
        }
    }

    public override void SpawnBullets()
    {
        base.SpawnBullets();
        AudioManager.instance.PlaySFX(shootSound);
    }
}
