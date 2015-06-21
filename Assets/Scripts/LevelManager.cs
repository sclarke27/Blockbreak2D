using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour
{

    private string levelName;
    private GameData gameData;
    private GameHUD gameHUD;
    private Cursor cursor;
    private Ball playerBall;
    

    void Awake()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
        gameHUD = GameObject.FindObjectOfType<GameHUD>();
        playerBall = GameObject.FindObjectOfType<Ball>();
    }

    public void LoadLevel(string name)
    {
        Brick.breakableCount = 0;
        gameData.PauseGame(false);
        levelName = name;
        if (levelName.IndexOf("Level") >= 0)
        {
            Screen.showCursor = false;
        }
        else
        {
            Screen.showCursor = true;
        }
        Application.LoadLevel(levelName);
    }

    public void StartGame()
    {
        Screen.showCursor = false;
        gameData.PauseGame(false);
        gameData.ResetPlayerLives();
        gameData.ResetPlayerScore();
        Brick.breakableCount = 0;
        Application.LoadLevel(1);
    }

    public void RestartLevel()
    {
        if (gameData.GetPlayerRemainingLives() <= 0)
        {
            gameData.ResetPlayerScore();
            gameData.ResetPlayerLives();
        }

        gameData.PauseGame(false);
        Brick.breakableCount = 0;
        Application.LoadLevel(Application.loadedLevel);
    }

    public void ResetPlayer()
    {
        //gameData.PauseGame(false);
        gameHUD.SetPlayerReady(false);
        playerBall.LockBall();
    }

    public void LoadNextLevel()
    {
        gameData.PauseGame(false);
        Brick.breakableCount = 0;
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void MainMenu()
    {
        Application.LoadLevel("StartMenu");
    }


    public void QuitRequest()
    {
        Application.Quit();
    }



}
