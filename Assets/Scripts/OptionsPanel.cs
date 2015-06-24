using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsPanel : MonoBehaviour
{

    public static OptionsPanel optionsPanel;

    private GameData gameData;

    // player pref UI Fields
    public Slider musicVolSlider;
    public Slider sfxVolSlider;
    public Slider paddleSpeedSlider;
    public Toggle useAIToggle;
    

    void Awake()
    {
        if (optionsPanel == null)
        {
            DontDestroyOnLoad(gameObject);
            optionsPanel = this;
        }
        else if (this != optionsPanel)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
        PopulateDefaultValues();
    }


    void Update()
    {
        
    }

    void PopulateDefaultValues()
    {
        if (gameData != null)
        {
            musicVolSlider.value = gameData.GetMusicVolume();
            sfxVolSlider.value = gameData.GetSFXVolume();
            paddleSpeedSlider.value = gameData.GetPlayerPaddleSpeed();
            useAIToggle.isOn = gameData.GetAIEnabled();
        }
    }

    public void SaveSelectedOptions()
    {
        gameData.SetMusicVolume(musicVolSlider.value);
        gameData.SetSFXVolume(sfxVolSlider.value);
        gameData.SetPlayerPaddleSpeed(paddleSpeedSlider.value);
        gameData.SetAIEnabled(useAIToggle.isOn);
        PopulateDefaultValues();
    }

    public void ShowOptionsPanel(bool showPanel)
    {
        gameObject.SetActive(showPanel);
        PopulateDefaultValues();
    }


}
