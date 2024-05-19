using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeManger : MonoBehaviour
{
    public static UpgradeManger instance;
    public Weapon shotgun;
    public Weapon sniper;
    public Weapon rifle;
    
    public Bullet shotgunBullet;
    public Bullet sniperBullet;
    public Bullet rifleBullet;

    public GameObject upgradeCanvas;
    public Image upgradeImageLeft;
    public Image upgradeImageRight;
    public TextMeshProUGUI upgradeTextLeft;
    public TextMeshProUGUI upgradeTextRight;
    
    public List<Upgrade> cyanUpgrades;
    public List<Upgrade> magentaUpgrades;
    public List<Upgrade> yellowUpgrades;

    private Dictionary<Element, Weapon> _weaponUpgradeDict;
    private Upgrade leftUpgrade;
    private Upgrade rightUpgrade;

    private void Awake()
    {
        instance = this;
        _weaponUpgradeDict = new Dictionary<Element, Weapon>
        {
            {Element.Cyan, sniper},
            {Element.Magenta, rifle},
            {Element.Yellow, shotgun}
        };
        
        SetBulletInfo(shotgunBullet);
        SetBulletInfo(sniperBullet);
        SetBulletInfo(rifleBullet);
    }

    public void CometMined(CometElement element)
    {
        upgradeCanvas.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        leftUpgrade = null;
        rightUpgrade = null;

        switch (element)
        {
            case CometElement.Red:
                leftUpgrade = magentaUpgrades[Random.Range(0, magentaUpgrades.Count)];
                rightUpgrade = yellowUpgrades[Random.Range(0, yellowUpgrades.Count)];
                upgradeImageLeft.color = Color.magenta;
                upgradeImageRight.color = Color.yellow;
                break;
            case CometElement.Blue:
                leftUpgrade = cyanUpgrades[Random.Range(0, cyanUpgrades.Count)];
                rightUpgrade = magentaUpgrades[Random.Range(0, magentaUpgrades.Count)];
                upgradeImageLeft.color = Color.cyan;
                upgradeImageRight.color = Color.magenta;
                break;
            case CometElement.Green:
                leftUpgrade = yellowUpgrades[Random.Range(0, yellowUpgrades.Count)];
                rightUpgrade = cyanUpgrades[Random.Range(0, cyanUpgrades.Count)];
                upgradeImageLeft.color = Color.yellow;
                upgradeImageRight.color = Color.cyan;
                break;
        }
        
        setUpgradeText(leftUpgrade, upgradeTextLeft);
        setUpgradeText(rightUpgrade, upgradeTextRight);
    }
    
    private void setUpgradeText(Upgrade upgrade, TextMeshProUGUI text)
    {
        text.text = "";
        if (upgrade.shellsPerShot != 0) text.text += ((upgrade.shellsPerShot > 0)? "+":"-")  + Mathf.Abs(upgrade.shellsPerShot) + " Shells Per Shot\n";
        if (upgrade.shotsPerBurst != 0) text.text += ((upgrade.shotsPerBurst > 0)? "+":"-")  + Mathf.Abs(upgrade.shotsPerBurst) + " Shots Per Burst\n";
        if (upgrade.fireRate != 0) text.text += ((upgrade.fireRate > 0)? "+":"-")  + Mathf.Abs(upgrade.fireRate) + " Bullets Per Minute\n";
        if (upgrade.accuracy != 0) text.text += ((upgrade.accuracy > 0)? "+":"-")  + Mathf.Abs(upgrade.accuracy) + "%" + " Accuracy\n";
        if (upgrade.timeBetweenBurstShots != 0) text.text += ((upgrade.timeBetweenBurstShots > 0)? "-":"+")  + Mathf.Abs(upgrade.timeBetweenBurstShots) + "%" + " Time Between Burst Shots\n";
        if (upgrade.bulletsInShell != 0) text.text += ((upgrade.bulletsInShell > 0)? "+":"-")  + Mathf.Abs(upgrade.bulletsInShell) + " Bullets In Shell\n";
        if (upgrade.energyCost != 0) text.text += ((upgrade.energyCost > 0)? "-":"+")  + Mathf.Abs(upgrade.energyCost) + "%" + " Energy Cost\n";
        if (upgrade.damage != 0) text.text += ((upgrade.damage > 0)? "+":"-")  + Mathf.Abs(upgrade.damage) + " Damage\n";
        if (upgrade.criticalDamage != 0) text.text += ((upgrade.criticalDamage > 0)? "+":"-")  + Mathf.Abs(upgrade.criticalDamage) + " Critical Damage\n";
        if (upgrade.speed != 0) text.text += ((upgrade.speed > 0)? "+":"-")  + Mathf.Abs(upgrade.speed) + " Bullet Speed\n";
        if (upgrade.shellAccuracy != 0) text.text += ((upgrade.shellAccuracy > 0)? "+":"-")  + Mathf.Abs(upgrade.shellAccuracy) + "%" + " Shell Accuracy\n";
        if (upgrade.gravity != 0) text.text += ((upgrade.gravity > 0)? "-":"+")  + Mathf.Abs(upgrade.gravity) + " Falloff\n";
    }

    private void ApplyUpgrade(Upgrade upgrade)
    {
        Weapon upgradeWeapon = _weaponUpgradeDict[upgrade.element];
        upgradeWeapon.shellsPerShot += upgrade.shellsPerShot;
        upgradeWeapon.shotsPerBurst += upgrade.shotsPerBurst;
        upgradeWeapon.fireRate += upgrade.fireRate;
        upgradeWeapon.inAccuracy *= (100 - upgrade.accuracy) / 100;
        upgradeWeapon.timeBetweenBurstShots *= (100 - upgrade.timeBetweenBurstShots) / 100;

        Bullet upgradeBullet = upgradeWeapon.bullet.GetComponent<Bullet>();
        upgradeBullet.bulletInfo.bulletsInShell += upgrade.bulletsInShell;
        upgradeBullet.bulletInfo.energyCost *= (100 - upgrade.energyCost) / 100;
        upgradeBullet.bulletInfo.damage += upgrade.damage;
        upgradeBullet.bulletInfo.criticalDamage += upgrade.criticalDamage;
        upgradeBullet.bulletInfo.speed += upgrade.speed;
        upgradeBullet.bulletInfo.inAccuracyX *= (100 - upgrade.shellAccuracy) / 100;
        upgradeBullet.bulletInfo.inAccuracyY *= (100 - upgrade.shellAccuracy) / 100;
        upgradeBullet.bulletInfo.gravity = Mathf.Clamp(upgradeBullet.gravity - upgrade.gravity, 0, 100);
    }

    public void UpgradeLeft()
    {
        ApplyUpgrade(leftUpgrade);
        TurnOffCanvas();
    }
    
    public void UpgradeRight()
    {
        ApplyUpgrade(rightUpgrade);
        TurnOffCanvas();
    }
    
    public void TurnOffCanvas()
    {
        upgradeCanvas.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SetBulletInfo(Bullet bullet)
    {
        bullet.bulletInfo.bulletsInShell = bullet.bulletsInShell;
        bullet.bulletInfo.energyCost = bullet.energyCost;
        bullet.bulletInfo.damage = bullet.damage;
        bullet.bulletInfo.criticalDamage = bullet.criticalDamage;
        bullet.bulletInfo.speed = bullet.speed;
        bullet.bulletInfo.inAccuracyX = bullet.inAccuracyX;
        bullet.bulletInfo.inAccuracyY = bullet.inAccuracyY;
        bullet.bulletInfo.lifeTime = bullet.lifeTime;
        bullet.bulletInfo.gravity = bullet.gravity;
    }
}
