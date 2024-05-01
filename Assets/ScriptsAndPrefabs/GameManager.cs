using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject player;
    public PlayerEnergy playerEnergy;
    public PlayerWeapon playerWeapon;
    public PlayerMovement playerMovement;
    public float defaultFOV;
    private void Awake()
    {
        // finds if there is an older InfoManager
        GameManager [] gm = FindObjectsByType<GameManager>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
      
        if (gm.Length > 1 && GameManager.instance != this)
        {
            Debug.Log(gm.Length);
            Destroy(gameObject);
        }
            
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerEnergy = player.GetComponent<PlayerEnergy>();
        playerWeapon = player.GetComponent<PlayerWeapon>();
        playerMovement = player.GetComponent<PlayerMovement>();
        defaultFOV = Camera.main.fieldOfView;
    }
}
