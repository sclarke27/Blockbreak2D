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
    public GoogleAnalyticsV3 googleAnalytics;
    

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
        gameData.googleAnalytics.LogEvent(new EventHitBuilder()
            .SetEventCategory("UIEvent")
            .SetEventAction("updateOptions")
            .SetEventLabel("Update Options"));
    }

    public void ShowOptionsPanel(bool showPanel)
    {
        if (showPanel)
        {
            googleAnalytics.LogScreen("Options Panel - Show");
        }
        else
        {
            googleAnalytics.LogScreen("Options Panel - Hide");
        }
        
        gameObject.SetActive(showPanel);
        PopulateDefaultValues();
    }


}
