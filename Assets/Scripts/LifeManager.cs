using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance { get; private set; }
    [SerializeField] public int lives;
    [SerializeField] private GameObject player;
    public static bool isInDeathSequence;
    public static bool isGameOvered = false;
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
        UIManager.instance.UpdateLives(lives);
        if (lives <= 0)
        {
            isGameOvered = true;
        }
        
        GameManager.instance.StartDeathSequence();
    }

    public void ResetLives()
    {
        lives = 3;
    }
}
