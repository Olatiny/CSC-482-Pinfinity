using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YingYangBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager.comboAdd = 0.0f;
        manager.setBumperMult(0);
        manager.setHeightModifier(3);
    }

    // Update is called once per frame
    void Update() { }
}
