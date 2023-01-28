using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private int score;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        scoreText.SetText(score.ToString());
    }

    public void AddScore(int value)
    {
        score += value;
    }
}
