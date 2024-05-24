using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        const int space_y = 401;
        transform.position += new Vector3(0, space_y, 0);
        GameObject.Find("Main Camera").transform.position += new Vector3(0, space_y, 0);
        GameObject.Find("BumperRow").transform.position += new Vector3(0, space_y, 0);
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        const float score_mod = 0.5f;
        manager.setScoreMult(score_mod);
        manager.setScoreModifier(-space_y * 100 * score_mod);
    }
}
