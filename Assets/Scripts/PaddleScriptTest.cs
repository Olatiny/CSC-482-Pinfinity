using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScriptTest : MonoBehaviour
{
    [SerializeField]
    private KeyCode key;

    [SerializeField]
    private KeyCode altKey;

    [SerializeField]
    private bool isMoving = false;

    [SerializeField]
    private float hitStrength = 100f;

    [SerializeField]
    private float damperStrength = 30f;

    private HingeJoint2D hinge;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(6, 3);
        Physics2D.IgnoreLayerCollision(7, 6);
    }

    public bool GetMoving()
    {
        //Debug.Log(isMoving ? "Moving!" : "Not Moving!");
        return isMoving;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Don't do anything if Paused or Game Over
        if (
            GameManager.Instance.state == GameManager.GameState.GameOver
            || GameManager.Instance.state == GameManager.GameState.Paused
        )
            return;

        if (Input.GetKey(key) || Input.GetKey(altKey))
        {
            GetComponent<Rigidbody2D>().AddTorque(hitStrength);
        }
        else
        {
            GetComponent<Rigidbody2D>().AddTorque(-damperStrength);
        }
    }
}
