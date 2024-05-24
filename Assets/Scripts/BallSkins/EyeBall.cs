using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        const int sky_y = 201;
        transform.position += new Vector3(0, sky_y, 0);
        GameObject.Find("Main Camera").transform.position += new Vector3(0, sky_y, 0);
        GameObject.Find("BumperRow").transform.position += new Vector3(0, sky_y, 0);
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        const float score_mod = 0.75f;
        manager.setScoreMult(score_mod);
        manager.setScoreModifier(-sky_y * 100 * score_mod);
    }

    // Update is called once per frame
    void Update() { }
}
