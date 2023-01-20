using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private Rigidbody2D Ball;

    private float lastXnonZero;

    [SerializeField] private int speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        Ball = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Paddle" && collision.gameObject.GetComponentInParent<PaddleScriptTest>().GetMoving())
        {
            Ball.AddForce(new Vector2(collision.gameObject.transform.up.normalized.x * speed, collision.gameObject.transform.up.normalized.y * speed), ForceMode2D.Force);
            //Debug.Log("After Paddle: x=" + Ball.velocity.x + " y=" + Ball.velocity.y);
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    Debug.Log("After Collision: x=" + Ball.velocity.x + " y=" + Ball.velocity.y);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            //Debug.Log("collided with wall");
            //Debug.Log("Velocity before: x=" + Ball.velocity.x + " y=" + Ball.velocity.y);
            Ball.AddForce(new Vector2(lastXnonZero * -2, 0), ForceMode2D.Force);
            //Debug.Log("Velocity after: x=" + Ball.velocity.x + " y=" + Ball.velocity.y);

        }
    }

    private void Update()
    {
        //Debug.Log("during update: x=" + Ball.velocity.x + " y=" + Ball.velocity.y);
        if (Ball.velocity.x != 0)
        {
            lastXnonZero = Ball.velocity.x;
        }

        if (!GetComponent<Renderer>().isVisible)
        {
            transform.position = new Vector2(1.26f, 0);
        }
    }
}
