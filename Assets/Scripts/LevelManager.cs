﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private string levelName;
    private GameData gameData;
    private GameHUD gameHUD;
    private Cursor cursor;
    private Ball playerBall;
    private MusicPlayer musicPlayer;

    //public GoogleAnalyticsV3 googleAnalytics;
    public GameAnalytics gameAnalytics;


    void Awake()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
        gameHUD = GameObject.FindObjectOfType<GameHUD>();
        playerBall = GameObject.FindObjectOfType<Ball>();
        musicPlayer = GameObject.FindObjectOfType<MusicPlayer>();
        
    }

    public void LoadLevel(string name)
    {
        gameData.SetPaddle("right", false);
        gameData.SetPaddle("left", false);

        
        Brick.breakableCount = 0;
        gameData.PauseGame(false);
        
        levelName = name;
        if (levelName.IndexOf("Level") >= 0)
        {
            musicPlayer.SetInMenu(false);
            Cursor.visible = false;
        }
        else
        {
            musicPlayer.SetInMenu(true);
            //we are in a menu screen
            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
            {
                Cursor.visible = true;
            }
            if (levelName == "LoseScreen")
            {
                //if player got high score, show name dialog instead of loading next level
                if (gameData.GetPlayerScoreRank() < 26)
                {
                    //gameData.SavePlayerScore();
                    gameHUD.ToggleHighScoreNameDialog(true, levelName);
                    return;
                }
                
            }
        }
        gameAnalytics.LogScreen(levelName);
        SceneManager.LoadScene(levelName);
    }

    public void StartGame()
    {
        Cursor.visible = false;
        gameData.PauseGame(false);
        gameData.ResetPlayerLives();
        gameData.ResetPlayerScore();
        musicPlayer.SetInMenu(false);
        gameData.SetPlayerReady(false);
        Brick.breakableCount = 0;
        SceneManager.LoadScene(1);
    }

    public void RestartLevel()
    {
        if (gameData.GetPlayerRemainingLives() <= 0)
        {
            gameData.ResetPlayerScore();
            gameData.ResetPlayerLives();
        }
        gameData.SetPaddle("right", false);
        gameData.SetPaddle("left", false);
        gameData.SetPlayerReady(false);

        gameData.PauseGame(false);
        Brick.breakableCount = 0;
        if (gameAnalytics != null)
        {
            gameAnalytics.LogEvent(GameAnalytics.gaEventCategories.GameEvent, "restartLevel", "Restart Level");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetPlayer()
    {
        //gameData.PauseGame(false);
        gameData.SetPaddle("right", false);
        gameData.SetPaddle("left", false);
        gameData.SetPlayerReady(false);
        playerBall.ShowBallDestruction();
        playerBall.LockBall();
    }

    public void LoadNextLevel()
    {
        musicPlayer.SetInMenu(false);
        gameData.PauseGame(false);
        gameData.SetPlayerReady(false);
        Brick.breakableCount = 0;
        if (gameAnalytics != null)
        {
            gameAnalytics.LogEvent(GameAnalytics.gaEventCategories.GameEvent, "nextLevel", "Load Next Leve");
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        LoadLevel("StartScreen");
        gameData.SetPlayerReady(false);
        musicPlayer.SetInMenu(true);
    }

    public void QuitRequest()
    {
        if (gameAnalytics != null)
        {
            gameAnalytics.LogEvent(GameAnalytics.gaEventCategories.GameEvent, "quitGame", "Quit Game");
        }
        Application.Quit();
    }

    public void ShowLevelComplete()
    {
        gameData.SetPaddle("right", false);
        gameData.SetPaddle("left", false);
        musicPlayer.SetInMenu(true);
        playerBall.ShowBallDestruction();
        playerBall.LockBall();
        gameData.PauseGame(true);
        gameHUD.ShowLevelComplete();
        gameData.SetPlayerReady(false);
        gameData.GainOneLife();
        if (gameAnalytics != null)
        {
            gameAnalytics.LogEvent(GameAnalytics.gaEventCategories.GameEvent, "levelComplete", SceneManager.GetActiveScene().name + " Complete", gameData.GetPlayerScore());
            gameAnalytics.LogScreen("Level Complete");
        }
    }


}
