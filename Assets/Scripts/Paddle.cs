using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour
{

    public float paddleScreenOffset = 0.5f;
    private float maxPaddleVelocity = 27.0f;
    private float acclerationAmount = 1.27f;
    private float declerationAmount = 4.27f;

    private float mouseXInBlocks;
    private float lastMouseXInBlocks;
    private Ball ball;
    private Vector3 paddleToBallVector;
    private GameData gameData;
    private bool useMouseInput = false;
    private float paddleVelocity = 0f;


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
        float newVelocityX = 0.0f;

        if (gameData.GetAIEnabled())
        {
            //paddlePos.x = Mathf.Clamp(ball.transform.position.x, paddleScreenOffset, gameData.levelUnitPixelWidth - paddleScreenOffset);
            //this.transform.position = paddlePos;
            if (this.transform.position.x > ball.transform.position.x)
            {
                gameData.SetPaddle("left", true);
            }
            else
            {
                gameData.SetPaddle("left", false);
            }
            if (this.transform.position.x < ball.transform.position.x)
            {
                gameData.SetPaddle("right", true);
            }
            else
            {
                gameData.SetPaddle("right", false);
            }
        }

        //accerate left
        if (gameData.IsLeftPaddledown() && !gameData.IsRightPaddledown())
        {
            if (paddleVelocity > 0f)
            {
                paddleVelocity = 0f;
            }
            newVelocityX = paddleVelocity - acclerationAmount;
            if (newVelocityX < (maxPaddleVelocity * -1))
            {
                newVelocityX = maxPaddleVelocity * -1;
            }
        }

        //accerate right
        if (gameData.IsRightPaddledown() && !gameData.IsLeftPaddledown())
        {
            if (paddleVelocity < 0f)
            {
                paddleVelocity = 0f;
            }
            newVelocityX = paddleVelocity + acclerationAmount;
            if (newVelocityX > maxPaddleVelocity)
            {
                newVelocityX = maxPaddleVelocity;
            }
        }

        //decelerate
        if (!gameData.IsRightPaddledown() && !gameData.IsLeftPaddledown() && paddleVelocity != 0.0f)
        {
            if (paddleVelocity > 0.0f)
            {
                newVelocityX = paddleVelocity - declerationAmount;
                if (newVelocityX < 0f)
                {
                    newVelocityX = 0;
                }
            }
            else if (paddleVelocity < 0.0f)
            {
                newVelocityX = paddleVelocity + declerationAmount;
                if (newVelocityX > 0f)
                {
                    newVelocityX = 0;
                }
            }
        }

        //stop at wall
        if ((this.transform.position.x >= gameData.levelUnitPixelWidth - paddleScreenOffset && newVelocityX > 0) || (this.transform.position.x <= paddleScreenOffset && newVelocityX < 0))
        {
            newVelocityX = 0.0f;
            Vector2 paddlePos = this.transform.position;
            paddlePos.x = Mathf.Clamp(paddlePos.x, paddleScreenOffset, gameData.levelUnitPixelWidth - paddleScreenOffset);
            this.transform.position = paddlePos;
        }

        //set paddle velocity
        paddleVelocity = newVelocityX;
        //Debug.Log(paddleVeolocity);
        //this.rigidbody2D.AddForce(new Vector2(paddleVelocity, 0.0f), ForceMode2D.Force);
        this.rigidbody2D.velocity = new Vector2(paddleVelocity, 0.0f);

        /*
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
                    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || gameData.IsLeftPaddledown())
                    {
                        paddlePos.x = paddlePos.x - (gameData.GetPlayerPaddleSpeed());
                    }
                    if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || gameData.IsRightPaddledown())
                    {
                        paddlePos.x = paddlePos.x + (gameData.GetPlayerPaddleSpeed());
                    }
                    paddlePos.x = Mathf.Clamp(paddlePos.x, paddleScreenOffset, gameData.levelUnitPixelWidth - paddleScreenOffset);
                    this.transform.position = paddlePos;
                }

            }
        }
         */
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        //Debug.Log(collision.collider.name);
        switch (collision.collider.name)
        {
            case "Ball":
                ball.AddSpin(this.rigidbody2D.velocity.x);
                break;
        }
    }
}
