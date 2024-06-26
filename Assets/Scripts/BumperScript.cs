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
    private GameObject particleEffect;

    [SerializeField]
    private GameObject particleEffectSecondary;

    [SerializeField]
    private Sprite defaultSprite;

    [SerializeField]
    private Sprite hitSprite;

    [SerializeField]
    private float forceStatic;

    [SerializeField]
    private int score;

    [SerializeField]
    private string color;

    private int numHits = 0;
    float timeToDestory = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            //GetComponent<AudioSource>().Play();
            switch (color)
            {
                case "red":
                    GameManager.Instance.soundManager.FXRedBumper();
                    break;
                case "green":
                    GameManager.Instance.soundManager.FXGreenBumper();
                    break;
                case "blue":
                    GameManager.Instance.soundManager.FXBlueBumper();
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

            ParticleSystem.Burst burst = new ParticleSystem.Burst(
                0f,
                (int)mag * 5 * GameManager.Instance.GetCombo() + 10
            );
            if (particleEffect)
            {
                particleEffect.GetComponent<ParticleSystem>().emission.SetBurst(0, burst);
                GameObject.Destroy(Instantiate(particleEffect, gameObject.transform), 1.0f);
            }
            if (particleEffectSecondary)
            {
                particleEffectSecondary.GetComponent<ParticleSystem>().emission.SetBurst(0, burst);
                GameObject.Destroy(
                    Instantiate(particleEffectSecondary, gameObject.transform),
                    1.0f
                );
            }

            //collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (GetComponent<Animator>())
            {
                GetComponent<Animator>().Play("BumperBounce");
            }

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
            if (numHits == hitsUntilDead)
            {
                timeToDestory = Time.time + .4f;
            }
        }
    }

    private void Update()
    {
        if (transform.position.y < Camera.main.transform.position.y - 10)
        {
            Destroy(gameObject);
        }
        if (timeToDestory != 0 && Time.time > timeToDestory)
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

        //if (GameManager.Instance.GetCombo() > 1)
        //{
        //    text.GetComponent<TextMesh>().text +=
        //        "\nx " + GameManager.Instance.GetCombo().ToString();
        //    Debug.Log(text.GetComponent<TextMesh>().text);
        //}
        text.GetComponent<TextMesh>().text = (
            (int)(GameManager.Instance.getBumperMult() * score * GameManager.Instance.GetCombo())
        ).ToString();
        float highlight = 0.5f;
        if (hitSprite)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprite;
        }
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
