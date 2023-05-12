using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
    [SerializeField]
    private Camera GameCamera;

    [SerializeField]
    private GameObject BallPrefab;

    [SerializeField]
    private GameObject paddles;

    [SerializeField]
    private Combo comboSystem;

    [SerializeField]
    private GameObject spawnPoint;

    [SerializeField]
    private LevelManager bumperManager;
    public SoundManager soundManager;

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
    private GameObject MainMenuCanvas;

    [SerializeField]
    private TextMeshProUGUI HighScoreTextMM;

    [SerializeField]
    private TextMeshProUGUI GameOverScoreText;

    [SerializeField]
    private TextMeshProUGUI HighScoreText;

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
            ScoreText.text = "0";
            LivesText.text = "0";

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
            ScoreText.SetText((HeightScore * 10 + BumperScore).ToString());
            LivesText.SetText(((int)HeightScore * 10).ToString());
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

    public class GameData
    {
        public Game[] game;
    }

    public class Game
    {
        public string date;
        public int score;
        public int machineId;
        public int id;
        public string userId;
        public int status;
    }

    static async Task PutScoreOnlineAsync(float data, LevelManager manager, bool newHighscore)
    {
        Debug.Log("posting");
        // Set the request URI
        string uri =
            "https://innate-carport-376722-kgs2kgxc6a-uc.a.run.app/freeflow-server/v1/venues/19227/machines/110/games";

        // Create an HttpClient object
        HttpClient client = new HttpClient();

        // Create a new post object
        var post = new { score = data.ToString() };
        // Convert the post object to JSON
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(post);
        // Create a StringContent object with the JSON
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        content.Headers.Add("score", data.ToString());
        // Send the POST request and wait for the response
        HttpResponseMessage response = await client.PutAsync(uri, content);
        // Read the response content as a string
        string responseBody = await response.Content.ReadAsStringAsync();
        Debug.Log("about to decode" + responseBody);
        // Find the index of the "id" field in the JSON string


        int idIndex = responseBody.IndexOf("id");
        // Find the end index of the "id" field value in the JSON string
        int idValueEndIndex = responseBody.IndexOf(',', idIndex);

        if (idValueEndIndex == -1)
        {
            idValueEndIndex = responseBody.IndexOf('}', idIndex);
        }

        // Extract the "id" field value from the JSON string

        int id = int.Parse(responseBody.Substring(idIndex + 4, idValueEndIndex - (idIndex + 4)));

        // Find the index of the "userId" field in the JSON string
        int userIdIndex = responseBody.IndexOf("userId");

        // Find the end index of the "userId" field value in the JSON string
        int userIdValueEndIndex = responseBody.IndexOf(',', userIdIndex);

        if (userIdValueEndIndex == -1)
        {
            userIdValueEndIndex = responseBody.IndexOf('}', userIdIndex);
        }

        // Extract the "userId" field value from the JSON string
        string userId = responseBody.Substring(
            userIdIndex + 9,
            userIdValueEndIndex - (userIdIndex + 10)
        );
        Debug.Log("here");
        // Use the values as needed
        Debug.Log("id: " + id);
        Debug.Log("userId: " + userId);

        // Output the response content
        bool gotToSky = manager.current_stage > 0;
        bool gotToSpace = manager.current_stage > 1;
        string gotToSkyUri =
            "https://innate-carport-376722-kgs2kgxc6a-uc.a.run.app/freeflow-server/v1/users/"
            + userId
            + "/achievements/13";
        string gotToSpaceUri =
            "https://innate-carport-376722-kgs2kgxc6a-uc.a.run.app/freeflow-server/v1/users/"
            + userId
            + "/achievements/14";
        string newHighscoreUri =
            "https://innate-carport-376722-kgs2kgxc6a-uc.a.run.app/freeflow-server/v1/users/"
            + userId
            + "/achievements/15";
        Debug.Log("urls made");
        if (gotToSky)
        {
            PutAchievment(gotToSkyUri, id, 1);
        }
        else
        {
            PutAchievment(gotToSkyUri, id, 0);
        }
        if (gotToSpace)
        {
            PutAchievment(gotToSpaceUri, id, 1);
        }
        else
        {
            PutAchievment(gotToSpaceUri, id, 0);
        }
        if (newHighscore)
        {
            PutAchievment(newHighscoreUri, id, 1);
        }
        else
        {
            PutAchievment(gotToSpaceUri, id, 0);
        }
    }

    static async Task PutAchievment(string uri, int gameID, int times)
    {
        Debug.Log("posting acheivment to gameid" + gameID);
        // Create an HttpClient object
        HttpClient client = new HttpClient();

        // Create a new post object
        var post = new { gameId = gameID.ToString(), timesAchieved = times.ToString() };
        // Convert the post object to JSON
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(post);
        // Create a StringContent object with the JSON
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        content.Headers.Add("gameId", gameID.ToString());
        content.Headers.Add("timesAchieved", times.ToString());
        // Send the POST request and wait for the response
        HttpResponseMessage response = await client.PutAsync(uri, content);
        // Read the response content as a string
        string responseBody = await response.Content.ReadAsStringAsync();
        Debug.Log(responseBody);
    }

    public void LoseLives(int lives)
    {
        this.lives -= lives;

        soundManager.FXDeath();

        if (this.lives <= 0)
        {
            bool newHighscore =
                (HeightScore * 10) + BumperScore > PlayerPrefs.GetInt("HighScore", 0);
            PutScoreOnlineAsync((HeightScore * 10 + BumperScore), bumperManager, newHighscore);
            PlayingCanvas.SetActive(false);
            GameOverCanvas.SetActive(true);
            GameOverScoreText.SetText(
                "Final Score:  " + (HeightScore * 10 + BumperScore).ToString()
            );
            // Check for new high score
            if (newHighscore)
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
        //HeightScore += 100000;
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
        HighScoreText.text = "High Score:  " + $"{PlayerPrefs.GetInt("HighScore", 0)}";
        HighScoreTextMM.text = "High Score:  " + $"{PlayerPrefs.GetInt("HighScore", 0)}";
    }
}
