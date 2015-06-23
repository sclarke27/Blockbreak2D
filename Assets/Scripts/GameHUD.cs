﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHUD : MonoBehaviour
{


    public string scoreText = "Score: ";
    public Text scoreTextField;
    public string livesText = "Lives: ";
    public Text livesTextField;
    public string pausedText = "Game\nPaused";
    public Text pausedTextField;
    public Text pausedTextFieldShadow;
    public Button restartButton;
    public Button quitButton;
    public Slider difficultySlider;
    public bool isMenuScreen;
    public bool isStartMenu = false;
    public Texture2D defaultCursor;
    public bool isFirstLevel = false;

    public GameObject pausePanel;
    public GameObject readyPanel;
    public GameObject levelCompletePanel;
    public GameObject instructionsPanel;
    public GameObject highScorePanel;
    public GameObject nameInputPanel;
    public Text highScoreNames;
    public Text highScoreScores;
    public InputField playerNameInput;

    private GameData gameData;
    //private LevelManager levelManager;
    private bool playerReady = false;
    private string nextLevel;
    private bool hasSeenInstructions = false;
    

    void Awake()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
        //levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    // Use this for initialization
    void Start()
    {
        
        
        if (!isMenuScreen)
        {
            scoreTextField.text = "";
            livesTextField.text = "";
            pausedTextField.text = "";
            pausedTextFieldShadow.text = "";
            pausePanel.SetActive(false);
            readyPanel.SetActive(false);
            instructionsPanel.SetActive(false);
        }
        else {
            pausePanel.SetActive(true);
            if (isStartMenu)
            {
                SetupHighScoresPanel();
                difficultySlider.value = gameData.GetDifficultyLevel();
            }
            if (Application.loadedLevelName == "WinScreen")
            {
                //if player got high score, show name dialog instead of loading next level
                if (gameData.GetPlayerScoreRank() < 26)
                {
                    //gameData.SavePlayerScore();
                    ToggleHighScoreNameDialog(true, "");
                    return;
                }

            }
        }
        
    }

    private void SetupHighScoresPanel() 
    {
        ArrayList highScores = gameData.GetHighScores();
        int scoreCount = (highScores.Count > 0) ? int.Parse(highScores[0].ToString().Split(',')[1]) : 0;
        Debug.Log(scoreCount);
        if (scoreCount == 0)
        {
            instructionsPanel.SetActive(true);
            highScorePanel.SetActive(false);
        }
        else
        {
            instructionsPanel.SetActive(false);
            highScorePanel.SetActive(true); 
            highScoreNames.text = "";
            highScoreScores.text = "";
            foreach (string score in highScores)
            {
                string[] scoreData = score.Split(',');
                highScoreNames.text += scoreData[0] + "\n";
                highScoreScores.text += scoreData[1] + "\n";
            }
        }
    }

    public void ClearHighScores()
    {
        gameData.DeleteHighScores();
        if (isStartMenu)
        {

            SetupHighScoresPanel();
        }
    }

    public void ToggleHighScoreNameDialog(bool showDialog, string levelName)
    {
        if (levelName != "")
        {
            nextLevel = levelName;
        }
        if (showDialog)
        {
            Screen.showCursor = true;
        }
        else
        {
            Screen.showCursor = false;
        }
        nameInputPanel.SetActive(showDialog);
    }

    public void SavePlayerHighScore()
    {
        if (playerNameInput.text != "")
        {
            Screen.showCursor = true;
            gameData.SavePlayerScore(playerNameInput.text);
            ToggleHighScoreNameDialog(false, "");
            if (nextLevel != "" && nextLevel != null)
            {
                Application.LoadLevel(nextLevel);
            }
        }
    }

    public void CancelPlayerHighScore()
    {
        ToggleHighScoreNameDialog(false, "");
        Application.LoadLevel(nextLevel);
    }

    public void SetPlayerReady(bool isReady)
    {
        playerReady = isReady;
        if (playerReady)
        {
            Screen.showCursor = false;
        }
    }

    public bool IsPlayerReady()
    {
        return playerReady;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMenuScreen && IsPlayerReady())
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                gameData.PauseGame(!gameData.IsGamePaused());
                if (gameData.IsGamePaused())
                {
                    Screen.showCursor = true;
                }
                else
                {
                    Screen.showCursor = false;
                }
            }

            scoreTextField.text = scoreText + gameData.GetPlayerScore();
            livesTextField.text = livesText + gameData.GetPlayerRemainingLives();

            if (gameData.IsGamePaused())
            {
                pausePanel.SetActive(true);
                pausedTextField.text = pausedText;
                pausedTextFieldShadow.text = pausedText;
            }
            else
            {
                pausePanel.SetActive(false);
                pausedTextField.text = "";
                pausedTextFieldShadow.text = "";
            }
            readyPanel.SetActive(false);
            instructionsPanel.SetActive(false);
        }
        else
        {
            if (!isMenuScreen)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Screen.showCursor = false;
                    hasSeenInstructions = true;
                    SetPlayerReady(true);
                }
                scoreTextField.text = scoreText + gameData.GetPlayerScore();
                livesTextField.text = livesText + gameData.GetPlayerRemainingLives();
                if (isFirstLevel && !hasSeenInstructions)
                {
                    instructionsPanel.SetActive(true);
                    readyPanel.SetActive(false);
                    
                }
                else
                {
                    instructionsPanel.SetActive(false);
                    readyPanel.SetActive(true);
                }
                
            }
            Screen.showCursor = true;
        }
    }

    public void HandleDiffcultySlider()
    {
        gameData.SetDifficulty(difficultySlider.value);
    }

    public void ShowLevelComplete()
    {
        Screen.showCursor = true;
        levelCompletePanel.SetActive(true);
    }
}
