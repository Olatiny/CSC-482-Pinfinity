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
        if (GameManager.Instance.state == GameManager.GameState.GameOver || GameManager.Instance.state == GameManager.GameState.Paused)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch1 = Input.GetTouch(0);

            if (touch1.phase == TouchPhase.Began || touch1.phase == TouchPhase.Stationary || touch1.phase == TouchPhase.Moved)
            {
                Vector2 pos1 = Camera.main.ScreenToWorldPoint(touch1.position);

                if (pos1.x > min_x_touch && pos1.x < max_x_touch)
                {
                    if (playSound)
                    {
                        playSound = false;
                        GetComponent<AudioSource>().Play();
                    }
                    //SoundManager.instance.PlaySoundEffect(SoundManager.instance.paddleSwing);
                    GetComponent<Rigidbody2D>().AddTorque(hitStrength);
                }
            }


            if (Input.touchCount >= 2)
            {
                Touch touch2 = Input.GetTouch(1);

                if (touch2.phase == TouchPhase.Began || touch2.phase == TouchPhase.Stationary || touch2.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Began || touch1.phase == TouchPhase.Stationary || touch1.phase == TouchPhase.Moved)
                {
                    Vector2 pos1 = Camera.main.ScreenToWorldPoint(touch1.position);
                    Vector2 pos2 = Camera.main.ScreenToWorldPoint(touch2.position);

                    if (pos1.x > min_x_touch && pos1.x < max_x_touch)
                    {
                        if (playSound)
                        {
                            playSound = false;
                            GetComponent<AudioSource>().Play();
                        }
                        //SoundManager.instance.PlaySoundEffect(SoundManager.instance.paddleSwing);
                        GetComponent<Rigidbody2D>().AddTorque(hitStrength);
                    }
                    else if (pos2.x > min_x_touch && pos2.x < max_x_touch)
                    {
                        if (playSound)
                        {
                            playSound = false;
                            GetComponent<AudioSource>().Play();
                        }
                        //SoundManager.instance.PlaySoundEffect(SoundManager.instance.paddleSwing);
                        GetComponent<Rigidbody2D>().AddTorque(hitStrength);
                    }
                }
            }
        } 
        else if (Input.GetKey(key) || Input.GetKey(altKey))
        {
            if (playSound)
            {
                playSound = false;
                GetComponent<AudioSource>().Play();
            } 
            //SoundManager.instance.PlaySoundEffect(SoundManager.instance.paddleSwing);
            GetComponent<Rigidbody2D>().AddTorque(hitStrength);
        }
        else
        {
            playSound = true;
            GetComponent<Rigidbody2D>().AddTorque(-damperStrength);
        }
    }
}
