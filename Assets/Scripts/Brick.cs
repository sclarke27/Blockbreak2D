﻿using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{

    public AudioClip breakSound;
    public AudioClip hitSound;
    public ParticleSystem breakParticles;
    public int scoreValue;
    public Sprite[] hitSprites;
    public bool isBonusBrick = false;
    public static int breakableCount = 0;

    private int timesHit;
    private LevelManager levelManager;
    private GameData gameData;
    private bool isBreakable;

    // Use this for initialization
    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        gameData = GameObject.FindObjectOfType<GameData>();

        isBreakable = (tag == "Breakable");
        if (isBreakable && !isBonusBrick)
        {
            breakableCount++;
        }
        timesHit = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DestroyBrick()
    {
        if (!isBonusBrick)
        {
            breakableCount--;
        }
        Destroy(gameObject);
        if (Brick.breakableCount <= 0)
        {
            levelManager.ShowLevelComplete();
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Ball")
        {
            int maxHits = hitSprites.Length + 1;
            float sfxVolume = gameData.GetSFXVolume();
            if (isBreakable)
            {
                timesHit++;
                showBreakParticles();
            }
            if (isBreakable && maxHits > 0 && timesHit >= maxHits)
            {
                DestroyBrick();
                gameData.AddPlayerScore(scoreValue);
                AudioSource.PlayClipAtPoint(breakSound, transform.position, sfxVolume);
            }
            else
            {
                LoadSprites();
                AudioSource.PlayClipAtPoint(hitSound, transform.position, sfxVolume);
            }
        }

    }

    void showBreakParticles()
    {

        ParticleSystem smoke = Instantiate(breakParticles, transform.position, Quaternion.identity) as ParticleSystem;
        smoke.Play();
        Destroy(smoke, smoke.main.duration);

    }

    void LoadSprites()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites.Length > 0 && hitSprites[spriteIndex])
        {
            if (hitSprites[spriteIndex] != null)
            {
                GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
            }
            else
            {
                Debug.LogError("[ERROR]: '" + name + "' block is missing sprite. index[" + spriteIndex + "]");
            }
        }
    }

    void SimulateWin()
    {
        levelManager.LoadNextLevel();
    }
}
