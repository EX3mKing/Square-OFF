using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class W_Enemy : Weapon
{
    [Header("ENEMY WEAPON SETTINGS")]
    [Tooltip("Shoot strait from (true) MUZZLE or directly at (false) PLAYER?")]
    public bool shootFromMuzzle;
    [Tooltip("returns the weapon to starting rotation when not aiming at player")]
    public bool returnWeapon;
    
    private Transform _target;
    private AudioSource _audioSource;

    public override void Start()
    {
        base.Start();
        isAutomatic = true;
        _target = GetComponentInParent<Enemy>().target;
        _audioSource = GetComponent<AudioSource>();
    }

    public override void LateUpdate()
    {
    }

    public override void Aiming()
    {
        // than aims at the player
        if (!isAiming)
        {
            if (returnWeapon) transform.localRotation = Quaternion.identity;
            return;
        }
        _target = GetComponentInParent<Enemy>().target;

        Vector3 direction = _target.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, aimSpeed * Time.deltaTime);
        Vector3 eulers = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulers.x, eulers.y, 0f);

        if (shootFromMuzzle) { bulletSpawnPoint.localRotation = Quaternion.identity;}
        else { bulletSpawnPoint.LookAt(_target); }
        ApplyInaccuracy();
    }
}
