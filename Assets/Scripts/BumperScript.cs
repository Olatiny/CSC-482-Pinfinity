using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BumperScript : MonoBehaviour
{
    private const int HITS_UNTIL_DEAD = 3;
    private const float KNOCKBACK_MULT = 1.5f;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GetComponent<AudioSource>().Play();

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
                        mag * normal.x + normal.x * forceStatic * GameManager.Instance.GetCombo(),
                        mag * normal.y + normal.y * forceStatic * GameManager.Instance.GetCombo()
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
            if (numHits >= 7)
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
        GameObject text = Instantiate(
            scoreText,
            gameObject.transform.position + new Vector3(0, 0.5f, 0),
            gameObject.transform.rotation
        );

        text.GetComponent<TextMesh>().alignment = TextAlignment.Center;

        if (GameManager.Instance.GetCombo() > 1)
        {
            text.GetComponent<TextMesh>().text += "\n(x " + GameManager.Instance.GetCombo().ToString() + ")";
            Debug.Log(text.GetComponent<TextMesh>().text);
        }

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
