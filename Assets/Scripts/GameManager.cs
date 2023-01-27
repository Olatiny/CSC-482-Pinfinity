using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool ballOnTop = false;

    [SerializeField] private GameObject ball;
    [SerializeField] private GameObject paddles;
    [SerializeField] private GameObject[] segments;
    [SerializeField] private GameObject spawnPoint;

    private Vector2 ballVelocity;
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
            score = (int) ball.transform.position.y;
        }
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
