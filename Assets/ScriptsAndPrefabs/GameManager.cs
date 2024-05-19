using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject player;
    public PlayerEnergy playerEnergy;
    public PlayerWeapon playerWeapon;
    public PlayerMovement playerMovement;
    public GameObject generator;
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
    }

    private void Start()
    {
        GetInfo();
    }

    public void GameOver()
    {
        SceneManager.LoadScene(2);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        GetInfo();
    }
    
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        GetInfo();
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GetInfo();
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    private void GetInfo()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        defaultFOV = Camera.main.fieldOfView;
        if (player == null) return;
        playerEnergy = player.GetComponent<PlayerEnergy>();
        playerWeapon = player.GetComponent<PlayerWeapon>();
        playerMovement = player.GetComponent<PlayerMovement>();
    }
    
}
