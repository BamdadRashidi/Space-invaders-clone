using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject MainMenu;
    public bool isInOptions = false;
    public UnityEngine.UI.Toggle vsyncToggle;
    public static MenuManager instance;
    
    private void Start()
    {
        OptionsMenu.SetActive(false);

        int savedVsync = PlayerPrefs.GetInt("VSync", 1);
        QualitySettings.vSyncCount = savedVsync;

        if (vsyncToggle != null)
            vsyncToggle.isOn = savedVsync == 1;

        ChangeScreenMode(PlayerPrefs.GetInt("ScreenMode", 1));
    }


    private void Update()
    {
        if (isInOptions && Input.GetKeyDown(KeyCode.Escape))
        {
            returnBackToMain();
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("Maingame");
    }

    public void option()
    {
        OptionsMenu.SetActive(true);
        MainMenu.SetActive(false);
        isInOptions = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void changeVSync(bool isEnabled)
    {
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
        PlayerPrefs.SetInt("VSync",QualitySettings.vSyncCount);
        switch (QualitySettings.vSyncCount)
        {
            case(0):
                Debug.Log("Vsync off");
                break;
            case(1):
                Debug.Log("Vsync on");
                break;
        }
        PlayerPrefs.Save();
    }

    public void ChangeResolution()
    {
        return;
    }

    public void ChangeScreenMode(int mode)
    {
        switch (mode)
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
        PlayerPrefs.SetInt("ScreenMode", mode);
        PlayerPrefs.Save();
    }

    public void TweakMaster()
    {
        return;
    }
    public void TweakSFX()
    {
        return;
    }
    public void TweakMusic()
    {
        return;
    }

    public void returnBackToMain()
    {
        OptionsMenu.SetActive(false);
        MainMenu.SetActive(true);
        isInOptions = false;
    }
}
