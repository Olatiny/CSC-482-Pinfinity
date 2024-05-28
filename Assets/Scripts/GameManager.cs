using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public const float SKY_BOUNDARY = 20000.0f;
    public const float SPACE_BOUNDARY = 40000.0f;

    public enum GameState
    {
        Intro,
        Playing,
        Paused,
        GameOver,
        MainMenu,
        Credits,
        Leaderboard,
    }

    public bool Paused
    {
        get { return state == GameState.GameOver || state == GameState.Paused; }
    }

    public static GameManager Instance;

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
    [SerializeField]
    private Camera GameCamera;

    [SerializeField]
    private GameObject paddles;

    [SerializeField]
    private Combo comboSystem;

    [SerializeField]
    private GameObject spawnPoint;

    [SerializeField]
    private LevelManager bumperManager;
    public SoundManager soundManager;

    [SerializeField]
    private LeaderboardManager leaderboardManager;

    [Header("UI fields")]
    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private TextMeshProUGUI HeightTexts;

    [SerializeField]
    private GameObject PlayingCanvas;

    [SerializeField]
    private GameObject PausedCanvas;

    [SerializeField]
    private GameObject GameOverCanvas;

    [SerializeField]
    private GameObject MainMenuCanvas;

    [SerializeField]
    private GameObject CreditsCanvas;

    [SerializeField]
    private GameObject LeaderboardCanvas;

    [SerializeField]
    private GameObject SettingsCanvas;

    [SerializeField]
    private GameObject SkinsCanvas;

    [SerializeField]
    private TextMeshProUGUI usernamePlaceholderText;

    [SerializeField]
    private TextMeshProUGUI usernameSubmitText;

    [SerializeField]
    private TextMeshProUGUI usernameErrorText;

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private TextMeshProUGUI HighScoreTextMM;

    [SerializeField]
    private TextMeshProUGUI GameOverScoreText;

    [SerializeField]
    private TextMeshProUGUI HighScoreText;

    [SerializeField]
    private SkinManager skinManager;

    [Header("Effects")]
    public float dimFactor = 1;
    private float ComboMult = 1;
    public float comboAdd = 0.5f;
    private Vector2 ballVelocity;
    private float ballAngularVel;
    private Color startingColor;
    private int BallHeight = 0;

    public int getBallHeight()
    {
        return BallHeight;
    }

    public void setBallHeight(int height)
    {
        BallHeight = height;
    }

    private int BumperScore = 0;
    public int lives = 1;
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
            BallHeight = 0;
            BumperScore = 0;
            paddles = GameObject.FindGameObjectWithTag("Paddles");
            spawnPoint = GameObject.FindGameObjectWithTag("BallSpawn");
            bumperManager = GameObject
                .FindGameObjectWithTag("BumperManager")
                .GetComponent<LevelManager>();
            GameCamera = Camera.main;
            startingColor = GameCamera.backgroundColor;
            soundManager.FXBlastOff();
            ScoreText.text = "0";
            HeightTexts.text = "0";

            UpdateHighScoreText();
            if (PausedCanvas)
                PausedCanvas.SetActive(false);
            if (GameOverCanvas)
                GameOverCanvas.SetActive(false);
            if (PlayingCanvas)
                PlayingCanvas.SetActive(false);
            if (MainMenuCanvas)
                MainMenuCanvas.SetActive(false);
            if (CreditsCanvas)
                CreditsCanvas.SetActive(false);
            if (LeaderboardCanvas)
                LeaderboardCanvas.SetActive(false);
            if (SettingsCanvas)
                SettingsCanvas.SetActive(false);
            if (SkinsCanvas)
                SkinsCanvas.SetActive(false);
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
            if (CreditsCanvas)
                CreditsCanvas.SetActive(false);
            if (LeaderboardCanvas)
                LeaderboardCanvas.SetActive(false);
            if (SettingsCanvas)
                SettingsCanvas.SetActive(false);
            if (SkinsCanvas)
                SkinsCanvas.SetActive(false);
            UpdateHighScoreText();
            state = GameState.MainMenu;
            soundManager.BGArcade();

            string s = PlayerPrefs.GetString("Skin");
            if (PlayerPrefs.GetString("Skin") == "")
                PlayerPrefs.SetString("Skin", "DefaultBall");

            skinManager.updateUI();
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
            if (CreditsCanvas)
                CreditsCanvas.SetActive(false);
            if (LeaderboardCanvas)
                LeaderboardCanvas.SetActive(false);
            if (SettingsCanvas)
                SettingsCanvas.SetActive(false);
            if (SkinsCanvas)
                SkinsCanvas.SetActive(false);
            soundManager.BGCutscene();
            UpdateHighScoreText();
            state = GameState.Intro;
        }
    }

    private void ResetBallMods()
    {
        comboAdd = 0.5f;
        heightMod = 1;
        setScoreModifier(0);
        bumperMult = 1;
    }

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
        ResetBallMods();
        ball = Instantiate(
            skinManager.getBall(),
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
        if (state == GameState.Credits)
            return;
        if (state == GameState.Leaderboard)
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
        if (Paused)
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

        BallHeight = Mathf.Max((int)(ball.transform.position.y * 100.0), BallHeight);
        if (BallHeight > 1)
        {
            ScoreText.SetText((GetTotalScore()).ToString());
            HeightTexts.SetText((BallHeight).ToString());
        }

        if (BallHeight > SKY_BOUNDARY && bumperManager.current_stage < 1)
        {
            bumperManager.current_stage = 1;
        }
        if (BallHeight > SPACE_BOUNDARY && bumperManager.current_stage < 2)
        {
            //Debug.Log("stage: " + bumperManager.current_stage);
            bumperManager.current_stage = 2;
            soundManager.BGSpace();
        }
        dimFactor = 1 - Mathf.Clamp((BallHeight - SKY_BOUNDARY) / SKY_BOUNDARY, 0f, 0.8f);
        GameCamera.backgroundColor = startingColor * dimFactor;
    }

    public void SetBallOnTop(bool top)
    {
        ballOnTop = top;
    }

    public void AddScore(int score)
    {
        this.BumperScore += (int)(ComboMult * score);
        ComboMult += comboAdd;
        comboSystem.UpdateCombo(ComboMult);
        soundManager.FXComboPitchUp();
        GetComponent<UnlockManager>().ComboUnlock(ComboMult);
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

    public async void LoseLives(int lives)
    {
        this.lives -= lives;
        PlayerPrefs.SetInt("deaths", PlayerPrefs.GetInt("deaths") + 1);
        soundManager.FXDeath();

        if (this.lives <= 0)
        {
            try
            {
                await leaderboardManager.AddScore(GetTotalScore());
            }
            catch { }

            PlayingCanvas.SetActive(false);
            GameOverCanvas.SetActive(true);
            GameOverScoreText.SetText("Final Score:  " + (GetTotalScore()).ToString());
            // Check for new high score
            if (GetTotalScore() > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", GetTotalScore());
                UpdateHighScoreText();
            }
            state = GameState.GameOver;
            GetComponent<UnlockManager>().CheckForUnlocks();
        }
        else
        {
            ball = Instantiate(
                skinManager.getBall(),
                spawnPoint.transform.position,
                spawnPoint.transform.rotation
            );
        }
    }

    private float score_modifier = 0;

    public void setScoreModifier(float modifier)
    {
        score_modifier = modifier;
    }

    public float heightMod = 1;

    public void setHeightModifier(float modifier)
    {
        heightMod = modifier;
    }

    public void setScoreMult(float mult)
    {
        bumperMult = mult;
        heightMod = mult;
    }

    private float bumperMult = 1;

    public void setBumperMult(float mult)
    {
        bumperMult = mult;
    }

    public float getBumperMult()
    {
        return bumperMult;
    }

    public int GetTotalScore()
    {
        return Mathf.Max(
            (int)((BallHeight * heightMod + BumperScore * bumperMult) + score_modifier),
            0
        );
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

    public void Credits()
    {
        state = GameState.Credits;
        PausedCanvas.SetActive(false);
        PlayingCanvas.SetActive(false);
        MainMenuCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        CreditsCanvas.SetActive(true);
        LeaderboardCanvas.SetActive(false);
        SettingsCanvas.SetActive(false);
        SkinsCanvas.SetActive(false);
    }

    public void Leaderboard()
    {
        state = GameState.Leaderboard;
        PausedCanvas.SetActive(false);
        PlayingCanvas.SetActive(false);
        MainMenuCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        LeaderboardCanvas.SetActive(true);
        SettingsCanvas.SetActive(false);
        SkinsCanvas.SetActive(false);

        leaderboardManager.UpdateScoreBoard();
    }

    public async void Settings()
    {
        state = GameState.Leaderboard;
        PausedCanvas.SetActive(false);
        PlayingCanvas.SetActive(false);
        MainMenuCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        LeaderboardCanvas.SetActive(false);
        SettingsCanvas.SetActive(true);
        SkinsCanvas.SetActive(false);

        usernameErrorText.text = "";
        usernamePlaceholderText.text = (await leaderboardManager.GetPlayerName()).Substring(
            0,
            (await leaderboardManager.GetPlayerName()).IndexOf('#')
        );
    }

    public void Skins()
    {
        state = GameState.Leaderboard;
        PausedCanvas.SetActive(false);
        PlayingCanvas.SetActive(false);
        MainMenuCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        LeaderboardCanvas.SetActive(false);
        SettingsCanvas.SetActive(false);
        SkinsCanvas.SetActive(true);
        skinManager.updateUI();
        leaderboardManager.UpdateScoreBoard();
    }

    public void SetVolume()
    {
        soundManager.SetVolume(volumeSlider.value);
    }

    public async void SetUsername()
    {
        string s = await leaderboardManager.UpdatePlayerName(usernameSubmitText.text);
        usernameErrorText.text = s;

        if (s == "success!")
        {
            usernameErrorText.color = Color.green;
        }
        else
        {
            usernameErrorText.color = Color.red;
        }
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
        HighScoreText.text = "High Score:  " + $"{PlayerPrefs.GetInt("HighScore", 0)}";
        HighScoreTextMM.text = "High Score:  " + $"{PlayerPrefs.GetInt("HighScore", 0)}";
    }
}
