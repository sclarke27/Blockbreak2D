﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour {

    public static GameData instance;
    public int levelUnitPixelWidth = 16;
    public float ballStartingVelocity = 7.0f;
    public float ballBounceMaxVelocity = 0.2f;
    public float ballBounceSideVariance = 0f;
    public float defaultSFXVolume = 1f;
    public float defaultMusicVolume = 0.14f;
    public float defaultDifficultyLevel = 1f;
    public float defaultPaddleSpeed = 0.1f;
    public int defaultPlayerLives = 5;

    private bool useAI = false;
    private float difficultyLevel = 2f;
    private int playerScore = 0;
    private int playerLives = 0;
    private bool gamePaused;
    private float currMusicVolume;
    private float currSFXVolume;
    private int totalHighScores = 25;
    private ArrayList highScoreList = new ArrayList();

    public enum playerPrefTypes
    {
        musicVolume,
        sfxVolume,
        useAI,
        difficultyLevel
    };

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            SetMusicVolume(PlayerPrefs.GetFloat(playerPrefTypes.musicVolume.ToString(), defaultMusicVolume));
            SetSFXVolume(PlayerPrefs.GetFloat(playerPrefTypes.sfxVolume.ToString(), defaultSFXVolume));
            SetAIEnabled( (PlayerPrefs.GetFloat(playerPrefTypes.useAI.ToString(), (!useAI) ? 0 : 1) == 0) ? false : true);
            SetDifficulty(PlayerPrefs.GetFloat(playerPrefTypes.difficultyLevel.ToString(), defaultDifficultyLevel));
            LoadHighScores();
            ResetPlayerLives();
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }
    }

    public void ResetPlayerScore()
    {
        playerScore = 0;
        Debug.Log("reset score");
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }

    public void DeleteHighScores()
    {
        Debug.Log("delete high scores");
        for (int i = 1; i <= totalHighScores; i++)
        {
            PlayerPrefs.DeleteKey("highscore" + i);
            //PlayerPrefs.DeleteAll();
        }
        highScoreList = new ArrayList();
        LoadHighScores();
    }

    public void LoadHighScores()
    {
        Debug.Log("LoadHighScores");
        string defaultStr = "******,0";
        for (int i = 1; i <= totalHighScores; i++)
        {
            string currentString = PlayerPrefs.GetString("highscore" + i);
            if (currentString.Length > 1)
            {
                highScoreList.Add(currentString);
                //Debug.Log("#" + i + ". " + currentString[0] + ":" + currentString[1]);
            }
            else
            {

                highScoreList.Add(defaultStr);
                //Debug.Log("No #" + i + " high score to load");
            }
        }
    }

    public int GetPlayerScoreRank()
    {
        int index = 0;
        foreach (string score in highScoreList)
        {
            string[] scoredata = score.Split(',');
            if (playerScore > int.Parse(scoredata[1]))
            {
                //highScoreList.Insert(index, playerName + "," + playerScore);
                return index;
            }
            index = index + 1;
        }

        return totalHighScores + 1;
    }

    public void SavePlayerScore(string playerName)
    {
        Debug.Log("Save Player Score:" + GetPlayerScore());
        int index = 0;
        foreach (string score in highScoreList)
        {
            string[] scoredata = score.Split(',');
            if(playerScore > int.Parse(scoredata[1])) {
                highScoreList.Insert(index, playerName + "," + playerScore);
                SaveHighScores();
                return;
            }
            index = index + 1;
        }
        
    }

    public ArrayList GetHighScores()
    {
        Debug.Log("Get high scores");
        return highScoreList;
    }

    public void SaveHighScores()
    {
        Debug.Log("Save high scores");
        int index = 1;
        foreach (string score in highScoreList)
        {
            if (score.IndexOf(',') > 1)
            {
                PlayerPrefs.SetString("highscore" + index, score);
                Debug.Log("#" + index + ". " + score);
            }
            index = index + 1;
        }

    }

    public void AddPlayerScore(int scoreValue)
    {
        playerScore = playerScore + scoreValue;
    }

    public bool IsGamePaused()
    {
        return gamePaused;
    }

    public void PauseGame(bool doPause)
    {
        gamePaused = doPause;
        if (gamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public bool GetAIEnabled()
    {
        return useAI;
    }

    public void SetAIEnabled(bool enableAI)
    {
        PlayerPrefs.SetFloat(playerPrefTypes.useAI.ToString(), (enableAI) ? 1 : 0);
        useAI = enableAI;
    }

    public float GetMusicVolume()
    {
        return currMusicVolume;
    }

    public void SetMusicVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(playerPrefTypes.musicVolume.ToString(), newVolume);
        currMusicVolume = newVolume;
    }

    public float GetSFXVolume()
    {
        return currSFXVolume;
    }

    public void SetSFXVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(playerPrefTypes.sfxVolume.ToString(), newVolume);
        currSFXVolume = newVolume;
    }

    public float GetDifficultyLevel()
    {
        return difficultyLevel;
    }

    public void SetDifficulty(float newDifficulty)
    {
        PlayerPrefs.SetFloat(playerPrefTypes.difficultyLevel.ToString(), newDifficulty);
        difficultyLevel = newDifficulty;
    }

    public float GetDefaultPaddleSpeed()
    {
        return defaultPaddleSpeed;
    }

    public int GetPlayerDefaultLives()
    {
        return defaultPlayerLives;
    }

    public int GetPlayerRemainingLives()
    {
        return playerLives;
    }

    private void SetPlayerRemainingLives(int lifeCount)
    {
        playerLives = lifeCount;
    }

    public void ResetPlayerLives()
    {
        playerLives = defaultPlayerLives;
    }

    public void LoseOneLife()
    {
        SetPlayerRemainingLives(playerLives - 1);
    }

    public void GainOneLife()
    {
        SetPlayerRemainingLives(playerLives + 1);
    }
}
