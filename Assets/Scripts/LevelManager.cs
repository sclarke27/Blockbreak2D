using UnityEngine;
using System.Collections;


public class LevelManager : MonoBehaviour
{

    private string levelName;
    private GameData gameData;
    private Cursor cursor;
    

    void Start()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
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
        Brick.breakableCount = 0;
        Application.LoadLevel(1);
    }

    public void RestartLevel()
    {
        gameData.PauseGame(false);
        Brick.breakableCount = 0;
        Application.LoadLevel(Application.loadedLevel);
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
