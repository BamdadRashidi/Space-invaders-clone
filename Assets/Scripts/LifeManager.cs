using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance { get; private set; }
    [SerializeField] public int lives;
    [SerializeField] private GameObject player;
    public bool isInDeathSequence;
    private void Awake()
    {
        isInDeathSequence = false;
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        player = GameObject.FindWithTag("Player");
    }
    
    void Update()
    {
        if (player.GetComponent<Player>().died && !isInDeathSequence)
        {
            reduceLifeAndContinue();
        }
    }

    public void reduceLifeAndContinue()
    {
        if (isInDeathSequence) return;
        lives--;
        isInDeathSequence = true;
        Debug.Log(lives);
        if (lives <= 0)
        {
            Debug.Log("Game Over");
            //TODO: add the Saving High-Score System and make the Game Over System
            ScoreManager.Instance.setHighScore(); // WIP
        }
        player.GetComponent<Player>().KillOrResurrect(); // add this to the end of Dying and respawning sequence but not to the game over
    }
}
