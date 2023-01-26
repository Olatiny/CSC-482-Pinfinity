using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BallScript : MonoBehaviour
{
    private Rigidbody2D Ball;

    private float lastXnonZero;

    [SerializeField] private int speed = 10;
    [SerializeField] private GameObject ballSpawn;
    [SerializeField] private Camera cam;

    //private float g = 0.25f;

    //private Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        Ball = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle") && collision.gameObject.GetComponentInParent<PaddleScriptTest>().GetMoving())
        {
            //float x_dist = Mathf.Abs(transform.position.x - collision.gameObject.transform.parent.position.x);
            //float y_dist = Mathf.Abs(transform.position.y - collision.gameObject.transform.parent.position.y);

            //Ball.AddForce(new Vector2(collision.gameObject.transform.up.normalized.x * speed * (1), collision.gameObject.transform.up.normalized.y * speed * (1)), ForceMode2D.Force);
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
            //Debug.Log("Velocity after: x=" + Ball.velocity.x + " y=" + Ball.velocity.y
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TopHalf")
        {
            //Debug.Log("top half!");
            GameManager.Instance.SetBallOnTop(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TopHalf")
        {
            //Debug.Log("no top half :(");
            GameManager.Instance.SetBallOnTop(false);
        }
    }

    private void Update()
    {
        //Ball.velocity = new Vector2(Ball.velocity.x, Ball.velocity.y - g);
        if (GameManager.Instance.ballOnTop && Ball.velocity.y > 0)
        {
            //Debug.Log("Cam x=" + cam.transform.position.x+ ", y=" + cam.transform.position.y);
            cam.transform.position = new Vector3(cam.transform.position.x, Ball.transform.position.y + .1f, cam.transform.position.z);
        }

        //Debug.Log("during update: x=" + Ball.velocity.x + " y=" + Ball.velocity.y);
        if (Ball.velocity.x != 0)
        {
            lastXnonZero = Ball.velocity.x;
        }

        if (!GetComponent<Renderer>().isVisible)
        {
            transform.position = ballSpawn.transform.position;
        }
    }
}
