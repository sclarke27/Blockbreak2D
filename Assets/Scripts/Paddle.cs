using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour
{

    public float paddleScreenOffset = 0.5f;

    private float mouseXInBlocks;
    private float lastMouseXInBlocks;
    private Ball ball;
    private Vector3 paddleToBallVector;
    private GameData gameData;
    private bool useMouseInput = false;

    // Use this for initialization
    void Start()
    {
        lastMouseXInBlocks = 0;// MouseCoordsToBlocks(Input.mousePosition.x);
        gameData = GameObject.FindObjectOfType<GameData>();
        ball = GameObject.FindObjectOfType<Ball>();
    }

    float MouseCoordsToBlocks(float mouseXcoord)
    {
        return ((mouseXcoord / Screen.width) * gameData.levelUnitPixelWidth);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameData.IsGamePaused())
        {
            Vector3 paddlePos = this.transform.position;
            if (gameData.GetAIEnabled())
            {
                paddlePos.x = Mathf.Clamp(ball.transform.position.x, paddleScreenOffset, gameData.levelUnitPixelWidth - paddleScreenOffset);
                this.transform.position = paddlePos;
            }
            else
            {
                if (useMouseInput)
                {
                    float currMouseXInBlocks = MouseCoordsToBlocks(Input.mousePosition.x);
                    float mouseXDelta = (lastMouseXInBlocks - currMouseXInBlocks);
                    mouseXInBlocks = currMouseXInBlocks + mouseXDelta;
                    paddlePos.x = Mathf.Clamp(mouseXInBlocks, paddleScreenOffset, gameData.levelUnitPixelWidth - paddleScreenOffset);
                    this.transform.position = paddlePos;
                    lastMouseXInBlocks = currMouseXInBlocks;
                }
                else
                {
                    float speedMultplier = gameData.GetDifficultyLevel() / 10;
                    if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        paddlePos.x = paddlePos.x - (gameData.GetDefaultPaddleSpeed() + speedMultplier);
                    }
                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        paddlePos.x = paddlePos.x + (gameData.GetDefaultPaddleSpeed() + speedMultplier);
                    }
                    paddlePos.x = Mathf.Clamp(paddlePos.x, paddleScreenOffset, gameData.levelUnitPixelWidth - paddleScreenOffset);
                    this.transform.position = paddlePos;
                }

            }
        }
        
    }
}
