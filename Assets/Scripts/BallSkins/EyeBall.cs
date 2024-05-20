using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(0, 205, 0);
        GameObject.Find("Main Camera").transform.position += new Vector3(0, 205, 0);
        GameObject.Find("BumperRow").transform.position += new Vector3(0, 205, 0);
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.setScoreMult(0.5f);
        manager.setScoreModifier(-20500);
    }

    // Update is called once per frame
    void Update() { }
}
