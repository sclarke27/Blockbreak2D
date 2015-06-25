using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameData : MonoBehaviour {

    public static GameData instance;
    public int levelUnitPixelWidth = 16;
    public float ballStartingVelocity = 7.0f;
    //public float ballBounceMaxVelocity = 0.2f;
    //public float ballBounceSideVariance = 0f;
    public float defaultSFXVolume = 1f;
    public float defaultMusicVolume = 0.14f;
    public float defaultDifficultyLevel = 1f;
    public float defaultPaddleSpeed = 0.3f;
    public int defaultPlayerLives = 5;
    public GoogleAnalyticsV3 googleAnalytics;

    private bool useAI = false;
    private float difficultyLevel = 2f;
    private int playerScore = 0;
    private int playerLives = 0;
    public float playerPaddleSpeed;
    private bool gamePaused;
    private float currMusicVolume;
    private float currSFXVolume;
    private int totalHighScores = 25;
    private ArrayList highScoreList = new ArrayList();

    private bool leftPaddledown = false;
    private bool rightPaddledown = false;

    public enum playerPrefTypes
    {
        musicVolume,
        sfxVolume,
        useAI,
        difficultyLevel,
        paddleSpeed
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
            SetPlayerPaddleSpeed(PlayerPrefs.GetFloat(playerPrefTypes.paddleSpeed.ToString(), defaultPaddleSpeed));
            LoadHighScores();
            ResetPlayerLives();
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        googleAnalytics.DispatchHits();
        googleAnalytics.StartSession();
    }

    public void ResetPlayerScore()
    {
        playerScore = 0;
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }

    public void DeleteHighScores()
    {
        for (int i = 1; i <= totalHighScores; i++)
        {
            PlayerPrefs.DeleteKey("highscore" + i);
        }
        highScoreList = new ArrayList();
        LoadHighScores();
    }

    public void LoadHighScores()
    {
        string defaultStr = "******,0";
        for (int i = 1; i <= totalHighScores; i++)
        {
            string currentString = PlayerPrefs.GetString("highscore" + i);
            if (currentString.Length > 1)
            {
                highScoreList.Add(currentString);
            }
            else
            {
                highScoreList.Add(defaultStr);
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
                return index;
            }
            index = index + 1;
        }

        return totalHighScores + 1;
    }

    public void SavePlayerScore(string playerName)
    {
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
        return highScoreList;
    }

    public void SaveHighScores()
    {
        int index = 1;
        foreach (string score in highScoreList)
        {
            if (score.IndexOf(',') > 1)
            {
                PlayerPrefs.SetString("highscore" + index, score);
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
        if (googleAnalytics != null)
        {
            googleAnalytics.LogEvent(new EventHitBuilder()
                .SetEventCategory("GameplayEvent")
                .SetEventAction("gamePaused")
                .SetEventLabel("Game Paused"));
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
        googleAnalytics.LogEvent(new EventHitBuilder()
            .SetEventCategory("UIEvent")
            .SetEventAction("aiEnabled")
            .SetEventLabel("AI Paddle Toggled")
            .SetEventValue((useAI) ? 1 : 0));

    }

    public float GetMusicVolume()
    {
        return currMusicVolume;
    }

    public GoogleAnalyticsV3 GetGA()
    {
        return googleAnalytics;
    }

    public void SetMusicVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(playerPrefTypes.musicVolume.ToString(), newVolume);
        currMusicVolume = newVolume;
        googleAnalytics.LogEvent(new EventHitBuilder()
            .SetEventCategory("UIEvent")
            .SetEventAction("musicVolume")
            .SetEventLabel("Set Music volume")
            .SetEventValue(System.Convert.ToInt64(currMusicVolume)));
    }

    public float GetSFXVolume()
    {
        return currSFXVolume;
    }

    public void SetSFXVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(playerPrefTypes.sfxVolume.ToString(), newVolume);
        currSFXVolume = newVolume;
        googleAnalytics.LogEvent(new EventHitBuilder()
            .SetEventCategory("UIEvent")
            .SetEventAction("sfxVolume")
            .SetEventLabel("Set SFX volume")
            .SetEventValue(System.Convert.ToInt64(currSFXVolume)));
    }

    public float GetDifficultyLevel()
    {
        return difficultyLevel;
    }

    public void SetDifficulty(float newDifficulty)
    {
        PlayerPrefs.SetFloat(playerPrefTypes.difficultyLevel.ToString(), newDifficulty);
        difficultyLevel = newDifficulty;
        googleAnalytics.LogEvent(new EventHitBuilder()
            .SetEventCategory("UIEvent")
            .SetEventAction("setDifficulty")
            .SetEventLabel("Set Difficulty")
            .SetEventValue(System.Convert.ToInt64(difficultyLevel)));
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
        googleAnalytics.LogEvent(new EventHitBuilder()
            .SetEventCategory("GameplayEvent")
            .SetEventAction("playerDied")
            .SetEventLabel("Player Died")
            .SetEventValue(GetPlayerRemainingLives()));
    }

    public void GainOneLife()
    {
        SetPlayerRemainingLives(playerLives + 1);
        googleAnalytics.LogEvent(new EventHitBuilder()
            .SetEventCategory("GameplayEvent")
            .SetEventAction("playerOneUp")
            .SetEventLabel("Player One Up")
            .SetEventValue(GetPlayerRemainingLives()));
    }

    public float GetPlayerPaddleSpeed()
    {
        return playerPaddleSpeed;
    }

    public void SetPlayerPaddleSpeed(float newSpeed)
    {
        playerPaddleSpeed = newSpeed;
        googleAnalytics.LogEvent(new EventHitBuilder()
            .SetEventCategory("UIEvent")
            .SetEventAction("setPaddleSpeed")
            .SetEventLabel("Set Paddle Speed")
            .SetEventValue(GetPlayerRemainingLives()));
    }

    public bool IsLeftPaddledown()
    {
        return leftPaddledown;
    }

    public bool IsRightPaddledown()
    {
        return rightPaddledown;
    }

    public void SetPaddle(string name, bool isdown)
    {
        switch (name)
        {
            case "left":
                leftPaddledown = isdown;
                break;
            case "right":
                rightPaddledown = isdown;
                break;

        }
    }

}
