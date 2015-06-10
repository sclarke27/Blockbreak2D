using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{

    public static MusicPlayer musicPlayer;
    public AudioSource bgMusic;
    private bool isPlaying = false;
    private GameData gameData;


    // Use this for initialization
    void Awake()
    {
        if (musicPlayer == null)
        {
            DontDestroyOnLoad(gameObject);
            musicPlayer = this;
        }
        else if (this != musicPlayer)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
        if (bgMusic != null && !isPlaying)
        {
            bgMusic.Play();
            isPlaying = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bgMusic.volume = gameData.GetMusicVolume();
        if (bgMusic != null && !isPlaying)
        {
            bgMusic.Play();
            isPlaying = true;
        }
    }
}
