using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperScript : MonoBehaviour
{
    private const int HITS_UNTIL_DEAD = 3;
    private const int KNOCKBACK_MULT = 2;

    [SerializeField]
    private GameObject scoreText;

    [SerializeField]
    private Sprite defaultSprite;

    [SerializeField]
    private Sprite hitSprite;

    [SerializeField]
    private float forceStatic;

    [SerializeField]
    private int score;
    private int numHits = 0;

    enum bumperColor
    {
        red,
        green, 
        blue
    }

    [SerializeField]
    private bumperColor color;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            switch (color)
            {
                case bumperColor.red:
                    SoundManager.instance.PlaySoundEffect(SoundManager.instance.redBumper);
                    break;
                case bumperColor.green:
                    SoundManager.instance.PlaySoundEffect(SoundManager.instance.greenBumper);
                    break;
                case bumperColor.blue:
                    SoundManager.instance.PlaySoundEffect(SoundManager.instance.blueBumper);
                    break;
                default:
                    break;
            }

            Vector2 normal = (
                collision.gameObject.transform.position - transform.position
            ).normalized;
            float mag = Mathf.Sqrt(
                collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude
            );
            //collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Animator>()
                .Play("BumperBounce");
            collision.gameObject
                .GetComponent<Rigidbody2D>()
                .AddForce(
                    new Vector2(
                        mag * normal.x + normal.x * forceStatic,
                        mag * normal.y + normal.y * forceStatic
                    ),
                    ForceMode2D.Impulse
                );
            if (numHits < HITS_UNTIL_DEAD)
            {
                forceStatic *= KNOCKBACK_MULT;
                StartCoroutine(BumperScore());
                GameManager.Instance.AddScore(score);
            }
            numHits++;
            if (numHits >= HITS_UNTIL_DEAD)
            {
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        if (transform.position.y < Camera.allCameras[0].transform.position.y - 10)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator BumperScore()
    {
        Instantiate(
            scoreText,
            gameObject.transform.position + new Vector3(0, 0.5f, 0),
            gameObject.transform.rotation
        );
        float highlight = 0.5f;
        GetComponent<SpriteRenderer>().sprite = hitSprite;
        float i = 0;
        while (i < highlight)
        {
            i += Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }
}
