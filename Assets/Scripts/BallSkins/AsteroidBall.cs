using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(0, 405, 0);
        GameObject.Find("Main Camera").transform.position += new Vector3(0, 405, 0);
        GameObject.Find("BumperRow").transform.position += new Vector3(0, 405, 0);
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.setScoreMult(0.5f);
        manager.setScoreModifier(-40500);
    }
}
