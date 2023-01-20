using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScriptTest : MonoBehaviour
{
    [SerializeField] private KeyCode key;

    [SerializeField] private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool GetMoving()
    {
        //Debug.Log(isMoving ? "Moving!" : "Not Moving!");
        return isMoving;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            gameObject.GetComponent<Animator>().Play("Paddle Hit");
        }

        if (Input.GetKeyUp(key))
        {
            gameObject.GetComponent<Animator>().Play("Paddle Release");
        }
    }
}
