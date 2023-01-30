using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedText : MonoBehaviour
{
    // Start is called before the first frame update
    private bool white = false;

    void Start()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        Color default_color = GetComponent<TextMesh>().color;
        float lifetime = 1.0f;
        const float COLOR_SWITCH = 0.25f;
        float color_iter = 0.0f;
        while (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
            color_iter += Time.deltaTime;
            gameObject.transform.position += new Vector3(0, Time.deltaTime * 0.5f, 0);
            if (color_iter > COLOR_SWITCH)
            {
                white = !white;
                color_iter = 0;
            }
            if (white)
            {
                GetComponent<TextMesh>().color = default_color;
            }
            else
            {
                GetComponent<TextMesh>().color = Color.white;
            }
            yield return null;
        }
        GameObject.Destroy(gameObject);
    }
}
