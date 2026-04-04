using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class PauseMenuManager : MonoBehaviour
{
    public bool isInOptions = false;
    public bool resumed = false;
    
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject sureMenu;
    [SerializeField] private UnityEngine.UI.Toggle vsyncToggle;
    [SerializeField] private TMP_Dropdown screenModeDD;
    [SerializeField] private UnityEngine.UI.Slider masterSlide;
    [SerializeField] private UnityEngine.UI.Slider musicSlide;
    [SerializeField] private UnityEngine.UI.Slider SFXslide;
    [SerializeField] private UnityEngine.UI.Button yes;
    [SerializeField] private UnityEngine.UI.Button no;
    private static string filePath;
    public static PauseMenuManager instance;
    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "Options.json");
        LoadOptions();
        if (masterSlide.value == 0)
        {
            mixer.SetFloat("MasterVolume", Mathf.Log10(masterSlide.maxValue) * 20);
        }
        if (musicSlide.value == 0)
        {
            mixer.SetFloat("MusicVolume", Mathf.Log10(musicSlide.maxValue) * 20);
        }
        if (SFXslide.value == 0)
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(SFXslide.maxValue) * 20);
        }
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        sureMenu.SetActive(false);
    }

    public void ActivatePause()
    {
        resumed = false;
        PauseMenu.SetActive(true);
    }

    public void activateOptions()
    {
        isInOptions = true;
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void returnBacktoMainFromOptions()
    {
        isInOptions = false;
        PauseMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }
    
    public void changeVSync()
    {
        QualitySettings.vSyncCount = vsyncToggle.isOn ? 1 : 0;
        SaveOptions();
    }

    public void ChangeResolution()
    {
        return;
    }

    public void ChangeScreenMode(int value)
    {
        // 0: Fullscreen, 1: Windowed, 2: Borderless
        switch (screenModeDD.value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Debug.Log("Full");
                break;

            case 1:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Debug.Log("Windowed");
                break;

            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Debug.Log("Borderless");
                break;
        }
        SaveOptions();
    }

    public void TweakMaster()
    {
        float value = masterSlide.value;
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        SaveOptions();
    }
    public void TweakSFX()
    {
        float value = SFXslide.value;
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        SaveOptions();
    }
    public void TweakMusic()
    {
        float value = musicSlide.value;
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        SaveOptions();
    }
    
    public void Resume()
    {
        resumed = true;
        isInOptions = false;
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
    }

    public void MainMenu()
    {
        PauseMenu.SetActive(false);
        sureMenu.SetActive(true);
    }

    public void yesQuit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void noDontQuit()
    {
        PauseMenu.SetActive(true);
        sureMenu.SetActive(false);
    }
    
    public void SaveOptions()
    {
        OptionsEntity data = new OptionsEntity()
        {
            VsyncMode = vsyncToggle.isOn ? 1 : 0,
            ScreenType = screenModeDD.value,
            MasterVolume = masterSlide.value,
            MusicVolume = musicSlide.value,
            SFXVolume = SFXslide.value
        };

        string jsonOptions = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath,jsonOptions);
    }

    public void LoadOptions()
    {
        OptionsEntity optionsData;
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            optionsData = JsonUtility.FromJson<OptionsEntity>(json);
            vsyncToggle.isOn = (optionsData.VsyncMode == 1);
            screenModeDD.value = optionsData.ScreenType;
            masterSlide.value = optionsData.MasterVolume;
            musicSlide.value = optionsData.MusicVolume;
            SFXslide.value = optionsData.SFXVolume;
            mixer.SetFloat("MasterVolume", Mathf.Log10(masterSlide.value) * 20);
            mixer.SetFloat("MusicVolume", Mathf.Log10(musicSlide.value) * 20);
            mixer.SetFloat("SFXVolume", Mathf.Log10(SFXslide.value) * 20);
        }
    }
    
}
