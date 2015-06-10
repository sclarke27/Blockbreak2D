using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHUD : MonoBehaviour
{

    public string gameTitle = "Block\nAttack";
    public string scoreText = "Score: ";
    public Text scoreTextField;
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

    private GameData gameData;
    private LevelManager levelManager;
    private bool playerReady = false;

    // Use this for initialization
    void Start()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
        if (!isMenuScreen)
        {
            scoreTextField.text = "";
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

    // Update is called once per frame
    void Update()
    {
        if (!isMenuScreen && playerReady)
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                gameData.PauseGame(!gameData.IsGamePaused());
            }

            scoreTextField.text = scoreText + gameData.GetPlayerScore();
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
                if (Input.GetMouseButtonDown(0))
                {
                    playerReady = true;
                }
                readyPanel.SetActive(true);
            }
            Screen.showCursor = true;
        }
    }

    public void HandleDiffcultySlider()
    {
        gameData.SetDifficulty(difficultySlider.value);
    }
}
