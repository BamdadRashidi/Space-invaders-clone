using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] tracks;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    public static MusicManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        musicSource = sources[0];
        sfxSource = sources[1];

        SceneManager.sceneLoaded += OnSceneLoaded;

        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    public void PlayMusicForScene(string sceneName)
    {
        if (sceneName == "MainMenu")
            musicSource.clip = tracks[0];

        else if (sceneName == "Maingame")
            musicSource.clip = tracks[1];

        musicSource.Play();
    }

    public void PauseTrack()
    {
        musicSource.Pause();
    }

    public void ResumeTrack()
    {
        musicSource.UnPause();
    }

    public void PlayVictory()
    {
        StartCoroutine(VictoryRoutine());
    }

    private IEnumerator VictoryRoutine()
    {
        musicSource.Pause();

        sfxSource.clip = tracks[2];
        sfxSource.Play();

        yield return new WaitForSeconds(sfxSource.clip.length + 0.5f);
        musicSource.UnPause();
    }
    
}
