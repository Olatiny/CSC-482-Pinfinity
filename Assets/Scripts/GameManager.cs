using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float SKY_DIM_MAX = 1000.0f;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        startingColor = gameCamera.backgroundColor;
    }

    public bool ballOnTop = false;

    [SerializeField]
    private Camera gameCamera;

    [SerializeField]
    private GameObject ball;

    [SerializeField]
    private GameObject paddles;

    [SerializeField]
    private GameObject[] segments;

    [SerializeField]
    private GameObject spawnPoint;

    private Vector2 ballVelocity;
    private Color startingColor;
    public float dimFactor = 1;
    public int score = 0;
    public int lives = 3;

    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
        MainMenu,
    }

    public GameState state = GameState.MainMenu;

    void Update()
    {
        ballVelocity = ball.GetComponent<Rigidbody2D>().velocity;

        if (ball.transform.position.y > score)
        {
            score = (int)ball.transform.position.y;
        }
        dimFactor = 1 - Mathf.Clamp(ball.transform.position.y / SKY_DIM_MAX, 0f, 0.8f);
        gameCamera.backgroundColor = startingColor * dimFactor;
    }

    public void SetBallOnTop(bool top)
    {
        ballOnTop = top;
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public void LoseLives(int lives)
    {
        this.lives -= lives;

        if (this.lives <= 0)
        {
            state = GameState.GameOver;
        }
    }

    public Vector2 GetBallVelocity()
    {
        return ballVelocity;
    }

    public void Pause()
    {
        state = GameState.Paused;
    }

    public void Resume()
    {
        state = GameState.Playing;
    }

    public void ResetGame()
    {
        state = GameState.MainMenu;

        score = 0;
        lives = 3;
    }
}
