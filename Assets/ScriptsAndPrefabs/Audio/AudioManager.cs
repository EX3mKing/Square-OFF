using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer audioMixer;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider spatialSlider;
    public Slider sensitivitySlider;
    public GameObject UICanvas;

    private void Awake()
    {
        instance = this;
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        spatialSlider.onValueChanged.AddListener(SetSpatialVolume);
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }
    
    private void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    
    private void SetSpatialVolume(float volume)
    {
        audioMixer.SetFloat("SpatialVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("SpatialVolume", volume);
    }
    
    private void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    
    private void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    
    private void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
        spatialSlider.value = PlayerPrefs.GetFloat("SpatialVolume", 1);
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 1);
        SetSensitivity(sensitivitySlider.value);
        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
        SetSpatialVolume(spatialSlider.value);
    }
    
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
    
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    
    public void Unfreeze()
    {
        UICanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
    
    public void Freeze()
    {
        UICanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Unfreeze();
            }
            else
            {
                Freeze();
            }
        }
    }
}
