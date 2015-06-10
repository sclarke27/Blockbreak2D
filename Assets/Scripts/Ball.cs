using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{

    private Paddle paddle;

    private Vector3 paddleToBallVector;
    private bool isLocked = true;
    private GameData gameData;

    // Use this for initialization
    void Start()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
        paddle = GameObject.FindObjectOfType<Paddle>();
        paddleToBallVector = this.transform.position - paddle.transform.position;
        //Debug.Log(optionsPanel.GetUserPreference("sfxVolume"));
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocked)
        {
            this.transform.position = paddle.transform.position + paddleToBallVector;
            if (Input.GetKeyDown(KeyCode.Space) || gameData.GetAIEnabled())
            {
                isLocked = false;
                this.rigidbody2D.velocity = new Vector2(gameData.ballBounceSideVariance, (gameData.ballStartingVelocity + gameData.GetDifficultyLevel()));
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || !gameData.GetAIEnabled())
            {
                isLocked = false;
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        float speedMultplier = gameData.GetDifficultyLevel() / 10;
        Vector2 tweak = new Vector2(0f, Random.Range(gameData.ballBounceSideVariance, gameData.ballBounceMaxVelocity + speedMultplier));
        this.rigidbody2D.velocity += tweak;
        if (!isLocked)
        {
            audio.volume = gameData.GetSFXVolume();
            audio.Play();
        }
    }
}
