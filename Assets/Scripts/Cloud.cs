using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed = .05f;
    public float depth = (float)Depth.CloseClouds;
    private Vector3 origialSpawnPos;
    private float _originalCameraPos;

    private void Update()
    {
        Color c = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(
            c.r,
            c.g,
            c.b,
            (FindObjectOfType<GameManager>().dimFactor / 2.5f)
        );
    }

    private void Start()
    {
        int flipped = Random.Range(0, 2) * 2 - 1;
        origialSpawnPos = new Vector3(transform.position.x * flipped, transform.position.y, depth);
    }
}
