using UnityEngine;
using System.Collections;

public class LoseCollider : MonoBehaviour
{

    private LevelManager levelManager;
    private GameData gameData;

    // Use this for initialization
    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        gameData = GameObject.FindObjectOfType<GameData>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        gameData.LoseOneLife();
        if (gameData.GetPlayerRemainingLives() <= 0)
        {
            levelManager.LoadLevel("LoseScreen");
        }
        else
        {
            levelManager.ResetPlayer();
        }

    }
}
