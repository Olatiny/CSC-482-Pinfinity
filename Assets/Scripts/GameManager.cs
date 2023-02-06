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

    //private void Awake()
    //{
    //    Instance = this;
    //    startingColor = GameCamera.backgroundColor;
    //}

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += (scene, mode) => OnSceneLoaded(scene, mode);
    }

    [Header("States and Flags")]
    public bool ballOnTop = false;
    public GameState state = GameState.MainMenu;

    [Header("Object References")]
    [SerializeField] private Camera GameCamera;
    [SerializeField] private GameObject BallPrefab;
    [SerializeField] private GameObject paddles;
    [SerializeField] private Combo comboSystem;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private LevelManager bumperManager;
    public SoundManager soundManager;

    [Header("UI fields")]
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI LivesText;
    [SerializeField] private GameObject PlayingCanvas;
    [SerializeField] private GameObject PausedCanvas;
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private TextMeshProUGUI HighScoreTextMM;
    [SerializeField] private TextMeshProUGUI GameOverScoreText;
    [SerializeField] private TextMeshProUGUI HighScoreText;

    [Header("Effects")]
    public float dimFactor = 1;
    private float ComboMult = 1;

    private Vector2 ballVelocity;
    private float ballAngularVel;
    private Color startingColor;
    private int HeightScore = 0;
    private int BumperScore = 0;
    private int lives = 1;
    private GameObject ball;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Instance != this)
        {
            return;
        }

        if (scene.name == "MainGame")
        {
            state = GameState.Intro;
            soundManager.Stop();

            lives = 1;
            HeightScore = 0;
            BumperScore = 0;
            paddles = GameObject.FindGameObjectWithTag("Paddles");
            spawnPoint = GameObject.FindGameObjectWithTag("BallSpawn");
            bumperManager = GameObject
                .FindGameObjectWithTag("BumperManager")
                .GetComponent<LevelManager>();
            GameCamera = Camera.allCameras[0];
            startingColor = GameCamera.backgroundColor;
            soundManager.FXBlastOff();

            UpdateHighScoreText();
            if (PausedCanvas)
                PausedCanvas.SetActive(false);
            if (GameOverCanvas)
                GameOverCanvas.SetActive(false);
            if (PlayingCanvas)
                PlayingCanvas.SetActive(false);
            if (MainMenuCanvas)
                MainMenuCanvas.SetActive(false);
            StartCoroutine(FadePaddles());
            Application.targetFrameRate = 60;
        }

        if (scene.name == "MainMenu")
        {
            if (PausedCanvas)
                PausedCanvas.SetActive(false);
            if (GameOverCanvas)
                GameOverCanvas.SetActive(false);
            if (PlayingCanvas)
                PlayingCanvas.SetActive(false);
            if (MainMenuCanvas)
                MainMenuCanvas.SetActive(true);
            UpdateHighScoreText();
            state = GameState.MainMenu;
            soundManager.BGArcade();
        }

        if (scene.name == "IntroCutscene")
        {
            if (PausedCanvas)
                PausedCanvas.SetActive(false);
            if (GameOverCanvas)
                GameOverCanvas.SetActive(false);
            if (PlayingCanvas)
                PlayingCanvas.SetActive(false);
            if (MainMenuCanvas)
                MainMenuCanvas.SetActive(false);
            soundManager.BGCutscene();
            UpdateHighScoreText();
            state = GameState.Intro;

        }
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

        for (float distance = length; distance > .1f; distance -= speed * Time.deltaTime)
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

        if (PlayingCanvas)
            PlayingCanvas.SetActive(true);
        state = GameState.Playing;
        paddles.GetComponentsInChildren<PaddleScriptTest>()[0].enabled = true;
        paddles.GetComponentsInChildren<PaddleScriptTest>()[1].enabled = true;
        soundManager.BGSky();
    }

    void Update()
    {
        if (state == GameState.Intro)
            return;
        if (state == GameState.GameOver)
            return;
        if (state == GameState.MainMenu)
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
        if (HeightScore > 1)
        {
            ScoreText.SetText("Score " + (HeightScore * 10 + BumperScore).ToString());
            LivesText.SetText("Height " + ((int)HeightScore).ToString());
        }

        if (HeightScore > 1000 && bumperManager.current_stage < 1)
        {
            bumperManager.current_stage = 1;
        }
        if (HeightScore > 2000 && bumperManager.current_stage < 2)
        {
            //Debug.Log("stage: " + bumperManager.current_stage);
            bumperManager.current_stage = 2;
            soundManager.BGSpace();
        }
        dimFactor = 1 - Mathf.Clamp((HeightScore - 1000) / SKY_DIM_MAX, 0f, 0.8f);
        GameCamera.backgroundColor = startingColor * dimFactor;
    }

    public void SetBallOnTop(bool top)
    {
        ballOnTop = top;
    }

    public void AddScore(int score)
    {
        this.BumperScore += (int)(ComboMult * score);
        ComboMult += 0.5f;
        comboSystem.UpdateCombo(ComboMult);
        soundManager.FXComboPitchUp();

        //Debug.Log("Combo: " + ComboMult);
    }

    public void ResetCombo()
    {
        ComboMult = 1f;
        comboSystem.UpdateCombo(ComboMult);
        soundManager.FXComboPitchReset();
        //Debug.Log("Combo (reset): " + ComboMult);
    }

    public float GetCombo()
    {
        return ComboMult;
    }

    public void LoseLives(int lives)
    {
        this.lives -= lives;

        soundManager.FXDeath();

        if (this.lives <= 0)
        {
            PlayingCanvas.SetActive(false);
            GameOverCanvas.SetActive(true);
            GameOverScoreText.SetText(
                "Final Score: " + (HeightScore * 10 + BumperScore).ToString()
            );
            // Check for new high score
            if ((HeightScore * 10) + BumperScore > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", HeightScore * 10 + BumperScore);
                UpdateHighScoreText();
            }
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
        soundManager.FXStop();
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("MainGame");
    }

    void UpdateHighScoreText()
    {
        HighScoreText.text = "High Score: " + $"{PlayerPrefs.GetInt("HighScore", 0)}";
        HighScoreTextMM.text = "High Score: " + $"{PlayerPrefs.GetInt("HighScore", 0)}";
    }
}
