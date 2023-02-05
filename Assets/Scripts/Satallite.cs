using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satallite : MonoBehaviour
{
    [SerializeField]
    private Sprite defaultSatallite;

    [SerializeField]
    private Sprite onSatallite;
    private bool lit;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeState());
    }

    IEnumerator ChangeState()
    {
        while (gameObject.activeSelf)
        {
            if (lit)
            {
                lit = !lit;

                GetComponent<SpriteRenderer>().sprite = onSatallite;
                yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
            }
            else
            {
                lit = !lit;

                GetComponent<SpriteRenderer>().sprite = defaultSatallite;
                yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
            }
        }
    }
}
