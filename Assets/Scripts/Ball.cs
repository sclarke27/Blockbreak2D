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

    private float minSpeed = 3f;
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

        //Debug.Log(optionsPanel.GetUserPreference("sfxVolume"));
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
            this.rigidbody2D.velocity = new Vector2(0f, (gameData.ballStartingVelocity + gameData.GetDifficultyLevel()));
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
            float currX = this.rigidbody2D.position.x;
            float currY = this.rigidbody2D.position.y;
            float newX = currX + velocityX;
            float newY = currY + velocityY;

            float diff = Mathf.Sqrt(Mathf.Pow((currX - newX), 2) + Mathf.Pow((currY - newY), 2));
            if (diff > maxSpeed)
            {
                //Debug.Log("X:" + velocityX + " Y:" + velocityY + " diff:" + diff);
                diff = diff - maxSpeed;
                if (velocityX * 1 > 10)
                {
                    velocityX = (velocityX > 0) ? (Mathf.Clamp((velocityX - diff), 0, velocityX)) : (Mathf.Clamp((velocityX + diff), velocityX, 0));
                }

                if (velocityY * 1 > 10)
                {
                    velocityY = (velocityY > 0) ? (Mathf.Clamp((velocityY - diff), 0, velocityY)) : (Mathf.Clamp((velocityY + diff), velocityY, 0));
                }

                //Debug.Log("X:" + velocityX + " Y:" + velocityY + " diff:" + diff);
                //Debug.Log("Too fast");
            }
            if (diff < minSpeed)
            {
                diff = maxSpeed - diff;
                if (velocityX * 1 > 10)
                {
                    velocityX = (velocityX > 0) ? (Mathf.Clamp((velocityX + diff), 0, velocityX)) : (Mathf.Clamp((velocityX - diff), velocityX, 0));
                }

                if (velocityY * 1 > 10)
                {
                    velocityY = (velocityY > 0) ? (Mathf.Clamp((velocityY + diff), 0, velocityY)) : (Mathf.Clamp((velocityY - diff), velocityY, 0));
                }

                //Debug.Log("X:" + velocityX + " Y:" + velocityY + " diff:" + diff);
                //Debug.Log("Too fast");
                Debug.Log("X:" + velocityX + " Y:" + velocityY + " diff:" + diff);
                Debug.Log("Too slow");
            }

            this.rigidbody2D.velocity = new Vector2(velocityX, velocityY);

        }
        else
        {
            this.transform.position = new Vector2(paddle.transform.position.x, paddle.transform.position.y + paddleToBallVector.y);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        //float speedMultplier = gameData.GetDifficultyLevel() / 10;
        //Vector2 tweak = new Vector2(0f, Random.Range(gameData.ballBounceSideVariance, gameData.ballBounceMaxVelocity + speedMultplier));
        float velocityX = this.rigidbody2D.velocity.x;
        float velocityY = this.rigidbody2D.velocity.y;
        float currX = this.rigidbody2D.position.x;
        float currY = this.rigidbody2D.position.y;
        float newX = currX + velocityX;
        float newY = currY + velocityY;

        float diff = Mathf.Sqrt(Mathf.Pow((currX- newX), 2) + Mathf.Pow((currY- newY), 2));

        if (velocityY >= 7.4 && velocityY <= 7.6)
        {
            velocityY = Mathf.Round(velocityY);
        }

        

        this.rigidbody2D.velocity = new Vector2(velocityX, velocityY);

        if (!isLocked)
        {
            audio.volume = gameData.GetSFXVolume();
            audio.Play();
        }
    }
}
