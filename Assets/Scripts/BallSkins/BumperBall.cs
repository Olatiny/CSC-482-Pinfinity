using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperBall : MonoBehaviour
{
    void Start()
    {
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.comboAdd = 1.0f;
        manager.heightMod = 0;
    }
}
