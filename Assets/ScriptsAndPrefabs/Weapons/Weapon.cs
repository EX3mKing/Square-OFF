using System;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// USE W_Base for most guns,
/// it is meant to be a base class for all weapons
/// Create separate classes for special/unique weapons
/// SWITCH CASE in Shoot() is MAYBE bad for performance on high fire rate weapons (not tested),
/// it is only here to make the dev process faster, not here for performance
/// </summary>
public class Weapon : MonoBehaviour
{
    [Tooltip("Bullets per second")] 
    public float fireRate;              // Bullets per second
    public float aimSpeed;              // How fast the weapon aims down sights
    [Tooltip("How many times does it magnify the view?")] [Range(0,12)]
    public float zoom;                  // !! GETS CALCULATED IN START and nowhere else!!
    public LayerMask aimMask;           // What layers does aim raycast hit
    
    [Range(0, 100)]                     // Gets averaged with the bullet's accuracy
    public float inAccuracy;            //  100+ is no accuracy, 0 is perfect accuracy
    public int shellsPerShot;           // How many shells are fired in a shot
    [Tooltip("Must be in BURST mode")]  // currently triggers burst only in BURST mode
    public int shotsPerBurst;           // How many shots are fired per BURST / CHARGE / SPECIAL
    public float timeBetweenBurstShots; // How much time is between the bullets in a SINGLE burst
    public bool isInaccuracyPerBullet;  // Should the inaccuracy be applied per bullet or per shot

    public bool isAimable;              // Can the weapon be aimed, like a sniper rifle
    public bool isAimToggle;            // when aiming do you need to hold the button or just press it for toggle
    public bool isLaserActive;          // Does the weapon have a laser attached
    public bool isAutomatic;            // Is the weapon automatic

    public AudioClip shootSound;
    public Color laserColor;
    public GameObject bullet;           // Reference to the bullet prefab
    protected Bullet bulletScript;        // Reference to the bullet script
    public Transform bulletSpawnPoint;
    
    public Type bulletType;
    public Element element;

    // formula for calculating the time between shots: 1 / fireRate 
    protected float shootCooldown;       // time between shots
    protected float shootCooldownTimer;  // counts down to 0, on 0 can shoot, resets after shooting
    private bool _isShootDown;          // is the shoot button held down

    // variables for aiming
    protected Camera mainCamera;
    protected Vector3 startPosition;     // used for aiming down sights
    public Transform middlePosition;    // position in the middle of the camera
    public bool isAiming;               // is the player aiming down sights
    protected float zoomFOV;             // the field of view when aiming down sights


    public enum Type
    {
        Pistol,
        Rifle,
        Shotgun,
        Sniper,
        Rocket,
        Melee,
        Special,
        None,
    }
    public enum Element
    {
        Cyan,
        Yellow,
        Magenta,
        Black, // can be used in any weapon slot, generally used for special weapons or melee weapons
    }

    public virtual void Start()
    { 
        Invoke("WaitForManager", 0.02f);
        shootCooldown = 1 / fireRate;
        startPosition = transform.localPosition;
        mainCamera = Camera.main;
        bulletScript = bullet.GetComponent<Bullet>();
    }
    
    private void WaitForManager()
    {
        // calculate the time between shots
        zoomFOV = GameManager.instance.defaultFOV / zoom;
    }
    

    public virtual void Update()
    {
        if (shootCooldownTimer > 0) shootCooldownTimer -= Time.deltaTime;
        else shootCooldownTimer = 0;

        Aiming();
    }

    public virtual void LateUpdate()
    {
        // cast a raycast to the point where the camera is looking at
        // rotate the bulletSpawnPoint to look at the hit point so the bullet fires straight to target
        // in late update to make sure the bulletSpawnPoint is in the right position
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, 
                mainCamera.transform.forward, out hit, 1000, aimMask))
        {
            bulletSpawnPoint.LookAt(hit.point);
        }
        else
        {
            bulletSpawnPoint.LookAt(mainCamera.transform.forward * 1000);
        }
        ApplyInaccuracy();
    }

    // THIS IS SINGLE TYPE SHOOTING
    public virtual void Shoot()
    {
        if (isAutomatic)
        {
            ShootBullets();
        }
        else if (!_isShootDown)
        {
            ShootBullets();
        }
        _isShootDown = true;
    }
    
    public virtual void StopShoot()
    {
        _isShootDown = false;
    }

    public virtual void ShootBullets()
    {
        if (shootCooldownTimer > 0) return; // if the cooldown is not 0, return
        shootCooldownTimer = shootCooldown; // reset the timer of the cooldown
        for (int k = 0; k < shotsPerBurst; k++)
        {
            Invoke("SpawnBullets", k * timeBetweenBurstShots);
        }
    }
    
    public virtual void SpawnBullets()
    {
        for (int i = 0; i < shellsPerShot; i++)
        {
            for (int j = 0; j < bulletScript.bulletsInShell ;j++)
            { 
                Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            }
            if (isInaccuracyPerBullet) ApplyInaccuracy();
        }
    }

    public virtual void Aim() // makes weapon aim down sights
    {
        if (!isAimable) return;                     // if the weapon is not aimable, return
        if (isAimToggle) isAiming = !isAiming;      // if the weapon is toggle aim, switch the aiming state
        else { isAiming = true; }                   // if the weapon is hold aim, set the aiming state to true
    }

    public virtual void StopAim()
    {
        if (!isAimable || isAimToggle) return;
        isAiming = false;
    }
    
    // NEEDS TO BE CALLED IN UPDATE TO WORK
    public virtual void Aiming() // moves the weapon and zooms the camera
    {
    }
    
    public virtual void ApplyInaccuracy()
    {
        // add inaccuracy to the shooting
        Vector2 random = UnityEngine.Random.insideUnitCircle;
        bulletSpawnPoint.rotation = Quaternion.Euler(new Vector3(random.x, random.y, 0 ) * inAccuracy + bulletSpawnPoint.rotation.eulerAngles);
    }
}
