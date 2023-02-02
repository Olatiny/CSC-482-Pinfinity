using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BumperScript : MonoBehaviour
{
    [SerializeField]
    private int hitsUntilDead = 5;
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
                        mag * normal.x + normal.x * forceStatic,
                        mag * normal.y + normal.y * forceStatic
                    ),
                    ForceMode2D.Impulse
                );
            if (numHits < hitsUntilDead)
            {
                forceStatic *= KNOCKBACK_MULT;
                StartCoroutine(BumperScore());
                GameManager.Instance.AddScore(score);
            }
            numHits++;
            if (numHits >= hitsUntilDead)
            {
                Destroy(gameObject, 0.25f);
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

    private float extraScoreHeight = 0;

    IEnumerator BumperScore()
    {
        extraScoreHeight += 0.5f;
        GameObject text = Instantiate(
            scoreText,
            gameObject.transform.position + new Vector3(0, extraScoreHeight, 0),
            gameObject.transform.rotation
        );
        float heightNow = extraScoreHeight;
        text.GetComponent<TextMesh>().alignment = TextAlignment.Center;

        // if (GameManager.Instance.GetCombo() > 1)
        // {
        //     text.GetComponent<TextMesh>().text +=
        //         "\nx " + GameManager.Instance.GetCombo().ToString();
        //     Debug.Log(text.GetComponent<TextMesh>().text);
        // }
        text.GetComponent<TextMesh>().text = (score * GameManager.Instance.GetCombo()).ToString();
        float highlight = 0.5f;
        GetComponent<SpriteRenderer>().sprite = hitSprite;
        float i = 0;
        while (i < highlight)
        {
            i += Time.deltaTime;
            yield return null;
        }
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
        if (heightNow == extraScoreHeight)
        {
            extraScoreHeight = 0;
        }
    }
}
