using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{

    public enum powerupTypes
    {
        oneUp,
        slowBall,
        fastBall
    };

    public powerupTypes powerUpType;
    public AudioSource bounceSound;

    private GameData gameData;

    // Use this for initialization
    void Start()
    {
        gameData = GameData.FindObjectOfType<GameData>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.IndexOf("Paddle") >= 0)
        {
            switch (powerUpType)
            {
                case powerupTypes.oneUp:
                    gameData.GainOneLife();
                    break;
            }
            Destroy(gameObject);

        }
        else
        {
            if (bounceSound != null)
            {
                bounceSound.Play();
            }
        }
    }
}
