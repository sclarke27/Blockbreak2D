using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{

    private Paddle paddle;
    private Vector3 paddleToBallVector;
    private bool isLocked = true;
    private GameData gameData;
    public AudioSource ballDestructionSound;
    public GameObject ballBreakParticles;


    private float minSpeed = 4f;
    private float maxSpeed = 10f;
    private ArrayList veloHistoryX = new ArrayList();
    private ArrayList veloHistoryY = new ArrayList();


    // Use this for initialization
    void Start()
    {
        gameData = GameObject.FindObjectOfType<GameData>();
        paddle = GameObject.FindObjectOfType<Paddle>();
        paddleToBallVector = this.transform.position - paddle.transform.position;
        veloHistoryX.Capacity = 5;
        veloHistoryY.Capacity = 5;
    }

    public void LockBall()
    {
        isLocked = true;
    }

    public void ShowBallDestruction()
    {
        if (ballDestructionSound != null)
        {
            ballDestructionSound.volume = gameData.GetSFXVolume();
            ballDestructionSound.Play();
        }
        ParticleEmitter breakParticles = Instantiate(ballBreakParticles, this.transform.position, Quaternion.identity) as ParticleEmitter;
        Destroy(breakParticles);


    }

    public void LaunchBall()
    {
        if (isLocked)
        {
            this.transform.position = paddle.transform.position + paddleToBallVector;
            isLocked = false;
            this.rigidbody2D.velocity = new Vector2(0f, (gameData.GetPlayerBallStartSpeed() + gameData.GetDifficultyLevel()));
        }
    }


    public void AddSpin(float spinAmount) {
        if (!isLocked)
        {
            //Debug.Log("Spin:" + spinAmount);
            this.rigidbody2D.AddTorque(spinAmount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocked)
        {
            float velocityX = this.rigidbody2D.velocity.x;
            float velocityY = this.rigidbody2D.velocity.y;
            //float currX = this.rigidbody2D.position.x;
            //float currY = this.rigidbody2D.position.y;
            //float newX = currX + velocityX;
            //float newY = currY + velocityY;
            float avgXSpeed = ((velocityX < 0) ? (velocityX * -1) : (velocityX));
            float avgYSpeed = ((velocityY < 0) ? (velocityY * -1) : (velocityY));
            float currAvgSpeed = (avgXSpeed + avgYSpeed) / 2;
            //Debug.Log(currAvgSpeed);

            float diff = 0;// Mathf.Sqrt(Mathf.Pow((currX - newX), 2) + Mathf.Pow((currY - newY), 2));
            if (currAvgSpeed > maxSpeed)
            {
                //Debug.Log("X:" + velocityX + " Y:" + velocityY + " diff:" + diff);
                diff = maxSpeed;
                if (velocityX * 1 > 10)
                {
                    velocityX = (velocityX > 0) ? (Mathf.Clamp((velocityX - diff), 0, velocityX)) : (Mathf.Clamp((velocityX + diff), velocityX, 0));
                }

                if (velocityY * 1 > 10)
                {
                    velocityY = (velocityY > 0) ? (Mathf.Clamp((velocityY - diff), 0, velocityY)) : (Mathf.Clamp((velocityY + diff), velocityY, 0));
                }

                //Debug.Log("X:" + velocityX + " Y:" + velocityY + " diff:" + diff);
                Debug.Log("Too fast");
            }
            if (currAvgSpeed < minSpeed)
            {
                diff = minSpeed;
                if (avgXSpeed <= minSpeed && avgYSpeed <= minSpeed)
                {
                    velocityX = (velocityX > 0) ? minSpeed : (minSpeed * -1); ;// (velocityX > 0) ? (Mathf.Clamp((velocityX + diff), 0, velocityX)) : (Mathf.Clamp((velocityX - diff), velocityX, 0));
                    velocityY = (velocityY > 0) ? minSpeed : (minSpeed * -1); ;//(velocityY > 0) ? (Mathf.Clamp((velocityY + diff), 0, velocityY)) : (Mathf.Clamp((velocityY - diff), velocityY, 0));
                }
                if (avgXSpeed < minSpeed && avgYSpeed > minSpeed)
                {
                    velocityX = (velocityX > 0) ? minSpeed : (minSpeed * -1);// (velocityX > 0) ? (Mathf.Clamp((velocityX + diff), 0, velocityX)) : (Mathf.Clamp((velocityX - diff), velocityX, 0));
                }
                if (avgYSpeed < minSpeed && avgXSpeed > minSpeed)
                {
                    velocityY = (velocityY > 0) ? minSpeed : (minSpeed * -1);// (velocityX > 0) ? (Mathf.Clamp((velocityX + diff), 0, velocityX)) : (Mathf.Clamp((velocityX - diff), velocityX, 0));
                }

                //Debug.Log("X:" + velocityX + " Y:" + velocityY + " diff:" + diff);
                //Debug.Log("Too fast");
                //Debug.Log("X:" + velocityX + " Y:" + velocityY + " diff:" + diff);
                Debug.Log("Too slow");
            }
            else
            {
                //Debug.Log("diff:" + diff);
            }

            this.rigidbody2D.velocity = new Vector2(velocityX, velocityY);

        }
        else
        {
            this.transform.position = new Vector2(paddle.transform.position.x, paddle.transform.position.y + paddleToBallVector.y);
            this.rigidbody2D.velocity = new Vector2(0f, 0f);
            this.rigidbody2D.rotation = 0f;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {

        if (!isLocked)
        {
            audio.volume = gameData.GetSFXVolume();
            audio.Play();
        }
    }
}
