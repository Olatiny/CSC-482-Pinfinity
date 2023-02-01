using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BallScript : MonoBehaviour
{
    private Rigidbody2D Ball;

    private float lastXnonZero;

    [SerializeField]
    private GameObject ballSpawn;

    private Vector2 lastVelocity = Vector2.zero;

    // Start is called before the first frame update
    private void Awake()
    {
        lastXnonZero = 0;
        Ball = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Ball.AddForce(new Vector2(lastXnonZero * -0.25f, 0), ForceMode2D.Force);
        }

        //if (collision.gameObject.CompareTag("Paddle") && collision.gameObject.GetComponent<Rigidbody2D>().angularVelocity != 0)
        //{
        //    SoundManager.instance.PlaySoundEffect(SoundManager.instance.paddleSwing);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TopHalf")
        {
            //Debug.Log("top half!");
            GameManager.Instance.SetBallOnTop(true);
        }

        if (collision.gameObject.CompareTag("BumperManager"))
        {
            GameManager.Instance.LoseLives(1);

            Destroy(gameObject);
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
        // Don't do anything if paused or game over.
        if (GameManager.Instance.state == GameManager.GameState.GameOver || GameManager.Instance.state == GameManager.GameState.Paused)
        {
            Ball.velocity = Vector2.zero;
            return;
        }

        if (Ball.velocity.x != 0)
        {
            lastXnonZero = Ball.velocity.x;
        }
    }
}
