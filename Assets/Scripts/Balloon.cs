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
    bool _popped = false;
    float _cooldown = .1f;

    void Start()
    {
        StartCoroutine(AnimateBalloon());
    }

    IEnumerator AnimateBalloon()
    {
        while (!_popped)
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
        while (_popped)
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
            _popped = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            GameObject.Destroy(gameObject, 2.0f);
        }
    }

    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            transform.position.y + 50
        );
        if (!_popped)
        {
            transform.position += Time.deltaTime * Vector3.up;
        }
        else
        {
            transform.position += Time.deltaTime * Vector3.down * 5;
        }
    }
}
