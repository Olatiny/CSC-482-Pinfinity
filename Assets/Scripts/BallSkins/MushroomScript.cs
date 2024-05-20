using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomScript : MonoBehaviour
{
    [SerializeField]
    GameObject otherMushy;

    void Start()
    {
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (manager.GetTotalScore() <= 0)
        {
            manager.lives = 2;
        }
        Destroy(otherMushy.GetComponent<MushroomScript>());
    }
}
