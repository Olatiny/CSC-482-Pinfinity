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
    public GameState state = GameState.Intro;

    [Header("Object References")]
    [SerializeField]
    private Camera GameCamera;

    [SerializeField]
    private GameObject BallPrefab;

    [SerializeField]
    private GameObject paddles;

    [SerializeField]
    private GameObject spawnPoint;

    [SerializeField]
    private BumperManager bumperManager;

    [Header("UI fields")]
    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private TextMeshProUGUI LivesText;

    [SerializeField]
    private GameObject PlayingCanvas;

    [SerializeField]
    private GameObject PausedCanvas;

    [SerializeField]
    private GameObject GameOverCanvas;

    [SerializeField]
    private TextMeshProUGUI GameOverScoreText;

    [Header("Effects")]
    public float dimFactor = 1;

    private float ComboMult = 1;

    private Vector2 ballVelocity;
    private float ballAngularVel;
    private Color startingColor;
    private int HeightScore = 0;
    private int BumperScore = 0;
    private int lives = 3;
    private GameObject ball;

    private void Start()
    {
        state = GameState.Intro;
        PausedCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        StartCoroutine(FadePaddles());
        SoundManager.instance.PlayBackground(SoundManager.instance.sky);
    }

    private bool enableCheck = true;

    IEnumerator FadePaddles()
    {
        float speed = 8f;
        float length = 16f;
        float final_paddle_left = paddles.transform.GetChild(0).transform.position.x;
        float final_holder_left = paddles.transform.GetChild(1).transform.position.x;
        float final_paddle_right = paddles.transform.GetChild(2).transform.position.x;
        float final_holder_right = paddles.transform.GetChild(3).transform.position.x;

        paddles.transform.GetChild(0).transform.position -= new Vector3(length, 0, 0);
        paddles.transform.GetChild(1).transform.position -= new Vector3(length, 0, 0);
        paddles.transform.GetChild(2).transform.position += new Vector3(length, 0, 0);
        paddles.transform.GetChild(3).transform.position += new Vector3(length, 0, 0);

        for (float distance = length; distance >= 0; distance -= speed * Time.deltaTime)
        {
            paddles.transform.GetChild(0).transform.position += new Vector3(
                speed * Time.deltaTime,
                0,
                0
            );
            paddles.transform.GetChild(1).transform.position += new Vector3(
                speed * Time.deltaTime,
                0,
                0
            );
            paddles.transform.GetChild(2).transform.position -= new Vector3(
                speed * Time.deltaTime,
                0,
                0
            );
            paddles.transform.GetChild(3).transform.position -= new Vector3(
                speed * Time.deltaTime,
                0,
                0
            );
            if (distance < 8f)
            {
                GameCamera.transform.position += new Vector3(0, Time.deltaTime * speed, 0);
            }
            yield return null;
        }
        ball = Instantiate(
            BallPrefab,
            spawnPoint.transform.position,
            spawnPoint.transform.rotation
        );
        paddles.transform.GetChild(0).transform.position = new Vector3(
            final_paddle_left,
            paddles.transform.GetChild(0).transform.position.y,
            3
        );
        paddles.transform.GetChild(1).transform.position = new Vector3(
            final_holder_left,
            paddles.transform.GetChild(1).transform.position.y,
            3
        );
        paddles.transform.GetChild(2).transform.position = new Vector3(
            final_paddle_right,
            paddles.transform.GetChild(2).transform.position.y,
            3
        );
        paddles.transform.GetChild(3).transform.position = new Vector3(
            final_holder_right,
            paddles.transform.GetChild(3).transform.position.y,
            3
        );

        state = GameState.Playing;
    }

    void Update()
    {
        if (state == GameState.Intro)
            return;
        if (state == GameState.GameOver)
            return;

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
        if (state == GameState.Paused)
            return;

        if (ballOnTop)
        {
            GameCamera.transform.position += new Vector3(
                0,
                (Mathf.Pow((ball.transform.position.y - GameCamera.transform.position.y), 2) / 3)
                    * Time.deltaTime,
                0
            );
        }

        HeightScore = Mathf.Max((int)(ball.transform.position.y * 10.0), HeightScore);
        ScoreText.SetText("Score: " + (HeightScore * 10 + BumperScore).ToString());
        LivesText.SetText("Height: " + ((int)HeightScore).ToString());
        if (HeightScore > 500 && bumperManager.current_stage < 1)
        {
            bumperManager.current_stage = 1;
        }
        if (HeightScore > 1000 && bumperManager.current_stage < 2)
        {
            //Debug.Log("stage: " + bumperManager.current_stage);
            bumperManager.current_stage = 2;
            SoundManager.instance.PlayBackground(SoundManager.instance.space);
        }
        dimFactor = 1 - Mathf.Clamp((HeightScore - 250) / SKY_DIM_MAX, 0f, 0.8f);
        GameCamera.backgroundColor = startingColor * dimFactor;
    }

    public void SetBallOnTop(bool top)
    {
        ballOnTop = top;
    }

    public void AddScore(int score)
    {
        this.BumperScore += (int) (ComboMult * score);
        if (ComboMult == 1)
            ComboMult = 1.25f;
        else
            ComboMult = 2 * ComboMult - 1;
        //Debug.Log("Combo: " + ComboMult);
    }

    public void ResetCombo()
    {
        ComboMult = 1f;
        //Debug.Log("Combo (reset): " + ComboMult);
    }

    public float GetCombo()
    {
        return ComboMult;
    }

    public void LoseLives(int lives)
    {
        this.lives -= lives;

        if (this.lives <= 0)
        {
            PlayingCanvas.SetActive(false);
            GameOverCanvas.SetActive(true);
            GameOverScoreText.SetText("Final Score: " + (HeightScore * 10 + BumperScore).ToString());
            state = GameState.GameOver;
        }
        else
        {
            ball = Instantiate(
                BallPrefab,
                spawnPoint.transform.position,
                spawnPoint.transform.rotation
            );
        }
    }

    public Vector2 GetLastBallVelocity()
    {
        return ballVelocity;
    }

    public void Pause()
    {
        //SoundManager.instance.Pause();
        state = GameState.Paused;
        Debug.Log("Paused Game");
        PlayingCanvas.SetActive(false);
        PausedCanvas.SetActive(true);
        ballVelocity = ball.GetComponent<Rigidbody2D>().velocity;
        ballAngularVel = ball.GetComponent<Rigidbody2D>().angularVelocity;
        ball.GetComponent<Rigidbody2D>().gravityScale = 0;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ball.GetComponent<Rigidbody2D>().angularVelocity = 0;
        //ball.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Resume()
    {
        //SoundManager.instance.UnPause();
        state = GameState.Playing;
        PausedCanvas.SetActive(false);
        PlayingCanvas.SetActive(true);
        ball.GetComponent<Rigidbody2D>().gravityScale = 1.6f;
        ball.GetComponent<Rigidbody2D>().velocity = ballVelocity;
        ball.GetComponent<Rigidbody2D>().angularVelocity = ballAngularVel;
        //ball.GetComponent<Rigidbody2D>().constraints = /*RigidbodyConstraints2D.None*/
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("MainGame");
    }
}
