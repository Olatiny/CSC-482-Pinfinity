using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    [SerializeField]
    bool resetLocksOnRun = false;

    void Start()
    {
        if (resetLocksOnRun)
        {
            PlayerPrefs.SetInt("LeadBall", 0);
            PlayerPrefs.SetInt("HappyBall", 0);
            PlayerPrefs.SetInt("MushroomBall", 0);
            PlayerPrefs.SetInt("SadBall", 0);
            PlayerPrefs.SetInt("EyeBall", 0);
            PlayerPrefs.SetInt("AsteroidBall", 0);
            PlayerPrefs.SetInt("YingYangBall", 0);
            PlayerPrefs.SetInt("BumperBall", 0);
        }
    }

    public void CheckForUnlocks()
    {
        GameManager manager = GetComponent<GameManager>();
        if (manager.GetTotalScore() > 10000)
        {
            PlayerPrefs.SetInt("LeadBall", 1);
        }
        if (manager.GetTotalScore() > 100000)
        {
            PlayerPrefs.SetInt("HappyBall", 1);
        }
        if (manager.GetTotalScore() > 150000)
        {
            PlayerPrefs.SetInt("MushroomBall", 1);
        }
        if (PlayerPrefs.GetInt("deaths") > 10)
        {
            PlayerPrefs.SetInt("SadBall", 1);
        }
        if (manager.getBallHeight() > 20000.0f)
        {
            PlayerPrefs.SetInt("EyeBall", 1);
        }
        if (manager.getBallHeight() > 40000.0f)
        {
            PlayerPrefs.SetInt("AsteroidBall", 1);
        }
        if (manager.GetTotalScore() <= 0)
        {
            PlayerPrefs.SetInt("YingYangBall", 1);
        }
    }

    public void ComboUnlock(float mult)
    {
        if (mult > 5.0f)
        {
            PlayerPrefs.SetInt("BumperBall", 1);
        }
    }

    public bool isUnlocked(string key)
    {
        return PlayerPrefs.GetInt(key) == 1;
    }
}
