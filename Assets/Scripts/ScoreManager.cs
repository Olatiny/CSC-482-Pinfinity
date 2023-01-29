using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private int scoreFromBumpers;
    private int scoreFromHeight;

    [SerializeField]
    private GameObject ball;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        scoreFromHeight = Mathf.Max((int)(ball.transform.position.y * 10.0), scoreFromHeight);
        int score = scoreFromBumpers + scoreFromHeight;
        scoreText.SetText(score.ToString());
    }

    public void AddScore(int value)
    {
        scoreFromBumpers += value;
    }
}
