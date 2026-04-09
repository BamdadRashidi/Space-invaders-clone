using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine.Audio;
public class MenuManager : MonoBehaviour
{
    public bool isInCredits = false;
    public bool isInOptions = false;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject Credits;
    [SerializeField] private GameObject waveSelection;
    [SerializeField] private UnityEngine.UI.Toggle vsyncToggle;
    [SerializeField] private TMP_Dropdown screenModeDD;
    [SerializeField] private UnityEngine.UI.Slider masterSlide;
    [SerializeField] private UnityEngine.UI.Slider musicSlide;
    [SerializeField] private UnityEngine.UI.Slider SFXslide;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private AudioMixer mixer;
    private bool isInMenu = false;
    private static string filePath;
    private static string savePath;
    public static MenuManager instance;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "Options.json");
        savePath = Path.Combine(Application.persistentDataPath, "Save.json");
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        Credits.SetActive(false);
        waveSelection.SetActive(false);
        LoadOptions();
    }
    
    private void Update()
    {
        if (isInOptions && Input.GetKeyDown(KeyCode.Escape))
        {
            returnBackToMain();
        }
        if (isInMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            returnBackToMain();
        }
    }

    public void Swap()
    {
        isInMenu = true;
        MainMenu.SetActive(false);
        waveSelection.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene("Maingame");
    }

    public void option()
    {
        title.gameObject.SetActive(false);
        OptionsMenu.SetActive(true);
        MainMenu.SetActive(false);
        isInOptions = true;
    }

    public void Quit()
    {
        Application.Quit();
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

    public void returnBackToMain()
    {
        title.gameObject.SetActive(true);
        waveSelection.SetActive(false);
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
        isInOptions = false;
    }

    public void ActivateCredits()
    {
        title.gameObject.SetActive(false);
        isInCredits = true;
        Credits.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void returnToMain()
    {    
        title.gameObject.SetActive(true);
        isInCredits = false;
        Credits.SetActive(false);
        MainMenu.SetActive(true);
    }

    private bool checkSaveFileContnet(SaveEntity entity)
    {
        return entity != null;
    }
    public void selectWave(int number)
    {
        SaveEntity ent = LoadSaveFile();
        switch (number)
        {
            case 1:
                break;
            case 2:
                if (checkSaveFileContnet(ent) && ent.highestWave >= 4)
                {
                    // TODO: add the method to set everything up
                }
                else
                {
                    Debug.Log("Haven't reached high wave");
                }
                break;
            case 4:
                if (checkSaveFileContnet(ent) && ent.highestWave >= 8)
                {
                    // TODO: add the method to set everything up
                }
                else
                {
                    Debug.Log("Haven't reached high wave");
                }
                break;
            case 6:
                if (checkSaveFileContnet(ent) && ent.highestWave >= 12)
                {
                    // TODO: add the method to set everything up
                }
                else
                {
                    Debug.Log("Haven't reached high wave");
                }
                break;
            case 8:
                if (checkSaveFileContnet(ent) && ent.highestWave >= 16)
                {
                    // TODO: add the method to set everything up
                }
                else
                {
                    Debug.Log("Haven't reached high wave");
                }
                break;
            case 10:
                if (checkSaveFileContnet(ent) && ent.highestWave >= 20)
                {
                    // TODO: add the method to set everything up
                }
                else
                {
                    Debug.Log("Haven't reached high wave");
                }
                break;
            case 12:
                if (checkSaveFileContnet(ent) && ent.highestWave >= 24)
                {
                    // TODO: add the method to set everything up
                }
                else
                {
                    Debug.Log("Haven't reached high wave");
                }
                break;
            case 15:
                if (checkSaveFileContnet(ent) && ent.highestWave >= 30)
                {
                    // TODO: add the method to set everything up
                }
                else
                {
                    Debug.Log("Haven't reached high wave");
                }
                break;
            Play();
        }
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

    private SaveEntity LoadSaveFile()
    {
        SaveEntity fileEntity = null;
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            fileEntity = JsonUtility.FromJson<SaveEntity>(json);
        }
        return fileEntity;
    }
}


public class OptionsEntity
{
    public int VsyncMode;
    public int ScreenType;
    public float MasterVolume;
    public float SFXVolume;
    public float MusicVolume;
}
