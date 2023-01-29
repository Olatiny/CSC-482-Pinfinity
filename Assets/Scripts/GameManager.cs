using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const float SKY_DIM_MAX = 1000.0f;

    public enum GameState
    {
        Intro,
        Playing,
        Paused,
        GameOver,
        MainMenu,
    }

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        startingColor = GameCamera.backgroundColor;
    }

    [Header("States and Flags")]
    public bool ballOnTop = false;
    public GameState state = GameState.Playing;

    [Header("Object References")]
    [SerializeField] private Camera GameCamera;
    [SerializeField] private GameObject BallPrefab;
    [SerializeField] private GameObject paddles;
    [SerializeField] private GameObject spawnPoint;

    [Header("UI fields")]
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI LivesText;
    [SerializeField] private GameObject PlayingCanvas;
    [SerializeField] private GameObject PausedCanvas;
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private TextMeshProUGUI GameOverScoreText;

    [Header("Effects")]
    public float dimFactor = 1;

    private Vector2 ballVelocity;
    private Color startingColor;
    private int HeightScore = 0;
    private int BumperScore = 0;
    private int lives = 3;
    private GameObject ball;

    private void Start()
    {
        state = GameState.Playing;
        PausedCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        ball = Instantiate(BallPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    void Update()
    {
        // If game over, don't do anything.
        if (state == GameState.GameOver) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (state)
            {
                case GameState.Paused:
                    Resume();
                    break;
                case GameState.Playing:
                    Pause();
                    break;
            }
        }

        // Don't do anything else while paused.
        if (state == GameState.Paused) return;

        if (ballOnTop && ball.GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            GameCamera.transform.position = new Vector3(GameCamera.transform.position.x, ball.transform.position.y + .1f, GameCamera.transform.position.z);
        }

        HeightScore = Mathf.Max((int)(ball.transform.position.y * 10.0), HeightScore);
        ScoreText.SetText("Score: " + (HeightScore + BumperScore).ToString());

        dimFactor = 1 - Mathf.Clamp((HeightScore / 10) / SKY_DIM_MAX, 0f, 0.8f);
        GameCamera.backgroundColor = startingColor * dimFactor;
    }



    public void SetBallOnTop(bool top)
    {
        ballOnTop = top;
    }

    public void AddScore(int score)
    {
        this.BumperScore += score;
    }

    public void LoseLives(int lives)
    {
        this.lives -= lives;

        LivesText.SetText("Lives: " + this.lives.ToString());

        if (this.lives <= 0)
        {
            PlayingCanvas.SetActive(false);
            GameOverCanvas.SetActive(true);
            GameOverScoreText.SetText("Final Score: " + (HeightScore + BumperScore).ToString());
            state = GameState.GameOver;
        } else
        {
            ball = Instantiate(BallPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }

    public Vector2 GetLastBallVelocity()
    {
        return ballVelocity;
    }

    public void Pause()
    {
        Debug.Log("Paused Game");
        PlayingCanvas.SetActive(false);
        PausedCanvas.SetActive(true);
        ballVelocity = ball.GetComponent<Rigidbody2D>().velocity;
        ball.GetComponent<Rigidbody2D>().gravityScale = 0;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        state = GameState.Paused;
    }

    public void Resume()
    {
        PausedCanvas.SetActive(false);
        PlayingCanvas.SetActive(true);
        ball.GetComponent<Rigidbody2D>().gravityScale = 1;
        ball.GetComponent<Rigidbody2D>().velocity = ballVelocity;
        state = GameState.Playing;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
