using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField]
    private Sprite[] poppedBallonSprites;

    [SerializeField]
    private Sprite[] floatingBallonSprites;

    int _ballonSpriteID = 0;
    public bool popped = false;
    float _cooldown = .05f;
    public float balloon_speed = 1.0f;

    void Start()
    {
        StartCoroutine(AnimateBalloon());
    }

    IEnumerator AnimateBalloon()
    {
        while (!popped)
        {
            GetComponent<SpriteRenderer>().sprite = floatingBallonSprites[_ballonSpriteID % 6];
            _ballonSpriteID++;
            yield return new WaitForSeconds(_cooldown + Random.Range(0, 0.3f));
        }

        GetComponent<SpriteRenderer>().sprite = poppedBallonSprites[0];
        yield return new WaitForSeconds(_cooldown);
        GetComponent<SpriteRenderer>().sprite = poppedBallonSprites[1];
        yield return new WaitForSeconds(_cooldown);
        GetComponent<SpriteRenderer>().sprite = poppedBallonSprites[2];
        yield return new WaitForSeconds(_cooldown);
        GetComponent<SpriteRenderer>().sprite = poppedBallonSprites[3];
        yield return new WaitForSeconds(_cooldown);
        _ballonSpriteID = 0;
        while (popped)
        {
            GetComponent<SpriteRenderer>().sprite = poppedBallonSprites[_ballonSpriteID % 2 + 4];
            _ballonSpriteID++;
            yield return new WaitForSeconds(_cooldown);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            popped = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            GameObject.Destroy(gameObject, 1.0f);
        }
    }

    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.y + 50
        );
        if (!popped)
        {
            transform.position += Time.deltaTime * Vector3.up * balloon_speed;
        }
        else
        {
            transform.position += Time.deltaTime * Vector3.down * balloon_speed * 5;
        }
    }
}
