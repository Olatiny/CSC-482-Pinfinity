using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScriptTest : MonoBehaviour
{
    [SerializeField]
    private KeyCode key;

    [SerializeField]
    private bool isMoving = false;

    [SerializeField]
    private float hitStrength = 100f;

    [SerializeField]
    private float damperStrength = 30f;

    private HingeJoint2D hinge;

    // Start is called before the first frame update
    void Start() { }

    public bool GetMoving()
    {
        //Debug.Log(isMoving ? "Moving!" : "Not Moving!");
        return isMoving;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(key))
        {
            GetComponent<Rigidbody2D>().AddTorque(hitStrength);
        }
        else
        {
            GetComponent<Rigidbody2D>().AddTorque(-damperStrength);
        }
    }
}
