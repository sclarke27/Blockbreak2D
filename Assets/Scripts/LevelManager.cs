using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour
{

    private string levelName;
    private GameData gameData;
    private GameHUD gameHUD;
    private Cursor cursor;
    private Ball playerBall;
    private MusicPlayer musicPlayer;
    public GoogleAnalyticsV3 googleAnalytics;
    

    void Awake()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
        gameHUD = GameObject.FindObjectOfType<GameHUD>();
        playerBall = GameObject.FindObjectOfType<Ball>();
        musicPlayer = GameObject.FindObjectOfType<MusicPlayer>();
        googleAnalytics.LogScreen(Application.loadedLevelName);
    }

    public void LoadLevel(string name)
    {
        Brick.breakableCount = 0;
        gameData.PauseGame(false);
        
        levelName = name;
        if (levelName.IndexOf("Level") >= 0)
        {
            musicPlayer.SetInMenu(false);
            Screen.showCursor = false;
        }
        else
        {
            musicPlayer.SetInMenu(true);
            Screen.showCursor = true;
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
        
        Application.LoadLevel(levelName);
    }

    public void StartGame()
    {
        Screen.showCursor = false;
        gameData.PauseGame(false);
        gameData.ResetPlayerLives();
        gameData.ResetPlayerScore();
        musicPlayer.SetInMenu(false);
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
        googleAnalytics.LogEvent(new EventHitBuilder()
            .SetEventCategory("UIEvent")
            .SetEventAction("restartLevel")
            .SetEventLabel("Restart Level"));

        Application.LoadLevel(Application.loadedLevel);
    }

    public void ResetPlayer()
    {
        //gameData.PauseGame(false);
        gameHUD.SetPlayerReady(false);
        playerBall.ShowBallDestruction();
        playerBall.LockBall();
    }

    public void LoadNextLevel()
    {
        musicPlayer.SetInMenu(false);
        gameData.PauseGame(false);
        Brick.breakableCount = 0;
        googleAnalytics.LogEvent(new EventHitBuilder()
            .SetEventCategory("GameplayEvent")
            .SetEventAction("nextLevel")
            .SetEventLabel("Load Next Level"));
        Application.LoadLevel(Application.loadedLevel + 1);


    }

    public void MainMenu()
    {
        musicPlayer.SetInMenu(true);
        LoadLevel("StartScreen");
    }

    public void QuitRequest()
    {
        googleAnalytics.LogEvent(new EventHitBuilder()
            .SetEventCategory("UIEvent")
            .SetEventAction("quitGame")
            .SetEventLabel("Quit Game"));
        Application.Quit();
    }

    public void ShowLevelComplete()
    {
        musicPlayer.SetInMenu(true);
        playerBall.ShowBallDestruction();
        playerBall.LockBall();
        gameData.PauseGame(false);
        gameHUD.ShowLevelComplete();
        googleAnalytics.LogEvent(new EventHitBuilder()
            .SetEventCategory("UIEvent")
            .SetEventAction("levelComplete")
            .SetEventLabel(Application.loadedLevelName + " Complete")
            .SetEventValue(gameData.GetPlayerScore()));
        googleAnalytics.LogScreen("Level Complete");
    }


}
