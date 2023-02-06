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

    [SerializeField]
    private float min_x_touch;

    [SerializeField]
    private float max_x_touch;

    private HingeJoint2D hinge;

    bool playSound = true;
    bool playRelease = true;

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

    bool playRoll = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (playRoll)
            {
                playRoll = false;
                GameManager.Instance.soundManager.FXPaddleBallRoll();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            //GameManager.Instance.soundManager.FXPaddleBallHit();
            playRoll = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Don't do anything if Paused or Game Over
        if (GameManager.Instance.state == GameManager.GameState.GameOver || GameManager.Instance.state == GameManager.GameState.Paused)
            return;

        if (Input.touchCount > 0)
        {
            playRelease = true;

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch t = Input.GetTouch(i);

                if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
                {
                    Vector2 pos1 = Camera.main.ScreenToWorldPoint(t.position);

                    if (pos1.x > min_x_touch && pos1.x < max_x_touch)
                    {
                        if (playSound)
                        {
                            playSound = false;
                            GameManager.Instance.soundManager.FXPaddleClick();
                        }
                        //SoundManager.instance.PlaySoundEffect(SoundManager.instance.paddleSwing);
                        GetComponent<Rigidbody2D>().AddTorque(hitStrength);
                    }
                }
            }
        } 
        else if (Input.GetKey(key) || Input.GetKey(altKey))
        {
            playRelease = true;

            if (playSound)
            {
                playSound = false;
                            GameManager.Instance.soundManager.FXPaddleClick();
            } 
            //SoundManager.instance.PlaySoundEffect(SoundManager.instance.paddleSwing);
            GetComponent<Rigidbody2D>().AddTorque(hitStrength);
        }
        else
        {
            playSound = true;

            if (playRelease)
            {
                playRelease = false;
                GameManager.Instance.soundManager.FXPaddleRelease();
            }

            GetComponent<Rigidbody2D>().AddTorque(-damperStrength);
        }
    }
}
