using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour
{

    
    public float paddleScreenOffset = 0.5f;

    private float maxPaddleVelocity = 20.0f;
    private float paddleVelocity = 0f;
    private float acclerationAmount = 0.04f;
    private float declerationAmount = 0.07f;
    private float currAccleration = 0f;
    private float currStep = 0;

    private Ball ball;
    private Vector3 paddleToBallVector;
    private GameData gameData;


    // Use this for initialization
    void Start()
    {
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

        if (((gameData.IsLeftPaddledown() && !gameData.IsRightPaddledown()) || (gameData.IsRightPaddledown() && !gameData.IsLeftPaddledown())) && currStep < 1f)
        {
            currStep = currStep + acclerationAmount;
        }

        if (!gameData.IsRightPaddledown() && !gameData.IsLeftPaddledown() && currStep > 0)
        {
            currStep = currStep - declerationAmount;
        }

        //Lerp acceleration amount
        currAccleration = Mathf.Lerp(0f, maxPaddleVelocity, currStep);
        //Debug.Log(currAccleration);
        
        //accerate left
        if (gameData.IsLeftPaddledown() && !gameData.IsRightPaddledown())
        {
            if (paddleVelocity > 0f)
            {
                paddleVelocity = 0f;
                currAccleration = 0f;
                currStep = 0f;
            }
            newVelocityX = (currAccleration * -1);
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
                currAccleration = 0f;
                currStep = 0f;
            }
            newVelocityX = currAccleration;
            if (newVelocityX > maxPaddleVelocity)
            {
                newVelocityX = maxPaddleVelocity;
            }
        }

        //decelerate
        if (!gameData.IsRightPaddledown() && !gameData.IsLeftPaddledown() && paddleVelocity != 0.0f)
        {
            //currAccleration = 0f;
            if (paddleVelocity > 0.0f)
            {
                newVelocityX = currAccleration;
                if (newVelocityX < 0f)
                {
                    newVelocityX = 0;
                    currAccleration = 0f;
                    currStep = 0;
                }
            }
            else if (paddleVelocity < 0.0f)
            {
                newVelocityX = (currAccleration * -1);
                if (newVelocityX > 0f)
                {
                    newVelocityX = 0;
                    currAccleration = 0f;
                    currStep = 0f;
                }
            }
        }
        if (!gameData.IsRightPaddledown() && !gameData.IsLeftPaddledown() && paddleVelocity == 0.0f)
        {
            currAccleration = 0f;
            currStep = 0f;
        }
        //stop at wall
        if ((this.transform.position.x >= gameData.levelUnitPixelWidth - paddleScreenOffset && newVelocityX > 0) || (this.transform.position.x <= paddleScreenOffset && newVelocityX < 0))
        {
            newVelocityX = 0.0f;
            Vector2 paddlePos = this.transform.position;
            paddlePos.x = Mathf.Clamp(paddlePos.x, paddleScreenOffset, gameData.levelUnitPixelWidth - paddleScreenOffset);
            this.transform.position = paddlePos;
            currAccleration = 0f;
        }

        //set paddle velocity
        paddleVelocity = newVelocityX;
        this.rigidbody2D.velocity = new Vector2(paddleVelocity, 0.0f);
        
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
