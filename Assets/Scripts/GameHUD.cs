using UnityEngine;
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
    public Texture2D defaultCursor;

    public GameObject pausePanel;
    public GameObject readyPanel;
    public GameObject levelCompletePanel;

    private GameData gameData;
    private LevelManager levelManager;
    private bool playerReady = false;

    void Awake()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
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
        }
        else {
            pausePanel.SetActive(true);
            if (difficultySlider != null)
            {
                difficultySlider.value = gameData.GetDifficultyLevel();
            }
        }
        
    }

    public void SetPlayerReady(bool isReady)
    {
        playerReady = isReady;
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
            }

            scoreTextField.text = scoreText + gameData.GetPlayerScore();
            livesTextField.text = livesText + gameData.GetPlayerRemainingLives();

            if (gameData.IsGamePaused())
            {
                Screen.showCursor = true;
                pausePanel.SetActive(true);
                pausedTextField.text = pausedText;
                pausedTextFieldShadow.text = pausedText;
            }
            else
            {
                Screen.showCursor = false;
                pausePanel.SetActive(false);
                pausedTextField.text = "";
                pausedTextFieldShadow.text = "";
            }
            readyPanel.SetActive(false);
        }
        else
        {
            if (!isMenuScreen)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetPlayerReady(true);
                }
                scoreTextField.text = scoreText + gameData.GetPlayerScore();
                livesTextField.text = livesText + gameData.GetPlayerRemainingLives();
                readyPanel.SetActive(true);
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
        levelCompletePanel.SetActive(true);
    }
}
