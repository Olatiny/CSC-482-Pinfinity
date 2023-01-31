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
    private float forceMult;

    [SerializeField]
    private float forceStatic;

    [SerializeField]
    private int score;
    private int numHits = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Vector2 dir = (collision.gameObject.transform.position - transform.position).normalized;

            //collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Animator>()
                .Play("BumperBounce");

            collision.gameObject
                .GetComponent<Rigidbody2D>()
                .AddForce(
                    new Vector2(
                        dir.x * forceMult + dir.x * forceStatic,
                        dir.y * forceMult + dir.y * forceStatic
                    ),
                    ForceMode2D.Impulse
                );
            forceStatic = forceStatic * KNOCKBACK_MULT;
            if (numHits < HITS_UNTIL_DEAD)
            {
                StartCoroutine(BumperScore());
                GameManager.Instance.AddScore(score);
                numHits++;
            }
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
