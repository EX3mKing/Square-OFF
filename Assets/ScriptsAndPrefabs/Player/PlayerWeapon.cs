using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public List<Weapon> weapons;
    public Weapon weapon;

    private void Start()
    {
        SetWeapon(0);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)){
            weapon.Shoot();
        }
        if (Input.GetMouseButtonUp(0))
        {
            weapon.StopShoot();
        }
        if (Input.GetMouseButtonDown(1)){
            weapon.Aim();
        }
        if (Input.GetMouseButtonUp(1)){
            weapon.StopAim();
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            ScrollWeapons(-1);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            ScrollWeapons(1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetWeapon(3);
    }

    private void ScrollWeapons(int direction)
    {
        int currentIndex = weapons.IndexOf(weapon);
        int nextIndex = currentIndex + direction;
        if (nextIndex < 0)
        {
            nextIndex = weapons.Count - 1;
        }
        else if (nextIndex >= weapons.Count)
        {
            nextIndex = 0;
        }
        SetWeapon(nextIndex);
    }

    private void SetWeapon(int weaponIndex)
    {
        if (weapon != null)
        {
            weapon.isAiming = false;
            weapon.StopShoot();
            weapon.gameObject.SetActive(false);
        }
        weapon = weapons[weaponIndex];
        weapon.gameObject.SetActive(true);
    }
}
