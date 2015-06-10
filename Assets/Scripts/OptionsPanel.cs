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
        Debug.Log("Music volume:" + gameData.GetMusicVolume());
        Debug.Log("SFX volume:" + gameData.GetSFXVolume());
        Debug.Log("Use AI:" + gameData.GetAIEnabled());
    }


    void PopulateDefaultValues()
    {
        if (gameData != null)
        {
            musicVolSlider.value = gameData.GetMusicVolume();
            sfxVolSlider.value = gameData.GetSFXVolume();
            useAIToggle.isOn = gameData.GetAIEnabled();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveSelectedOptions()
    {
        gameData.SetMusicVolume(musicVolSlider.value);
        gameData.SetSFXVolume(sfxVolSlider.value);
        gameData.SetAIEnabled(useAIToggle.isOn);
    }

    public void ShowOptionsPanel(bool showPanel)
    {
        gameObject.SetActive(showPanel);
        PopulateDefaultValues();
    }


}
