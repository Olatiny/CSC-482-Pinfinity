using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFlicker : MonoBehaviour
{
    private bool lit = true;
    private Color startingColor;

    // Start is called before the first frame update
    void Start()
    {
        startingColor = GetComponent<SpriteRenderer>().color;
        StartCoroutine(FlickerStar());
    }

    IEnumerator FlickerStar()
    {
        while (gameObject.activeSelf)
        {
            if (lit)
            {
                StartCoroutine(Dim());
            }
            else
            {
                StartCoroutine(Brighten());
            }
            lit = !lit;
            yield return new WaitForSeconds(Random.Range(5.0f, 15.0f));
        }
    }

    IEnumerator Brighten()
    {
        for (float i = 0.5f; i < 1.0f; i += .1f)
        {
            GetComponent<SpriteRenderer>().color = startingColor * i;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator Dim()
    {
        for (float i = 1.0f; i > 0.5f; i -= .1f)
        {
            GetComponent<SpriteRenderer>().color = startingColor * i;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
