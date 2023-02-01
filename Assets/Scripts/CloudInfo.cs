using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudInfo : MonoBehaviour
{
    [SerializeField]
    private float depth = 1;
    private Vector3 origialSpawnPos;

    private void Update()
    {
        Transform cameraTransform = Camera.allCameras[0].transform;
        float distanceFromCamera = cameraTransform.position.y - origialSpawnPos.y;
        transform.position = new Vector3(
            transform.position.x + Time.deltaTime * (1 / depth) * 0.1f,
            origialSpawnPos.y + distanceFromCamera / depth,
            origialSpawnPos.z
        );
        Color c = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = new Color(
            c.r,
            c.g,
            c.b,
            (FindObjectOfType<GameManager>().dimFactor / 2.5f)
        );
        if (transform.position.y < cameraTransform.position.y - 10)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        int flipped = Random.Range(0, 2) * 2 - 1;
        origialSpawnPos = new Vector3(
            transform.position.x * flipped,
            transform.position.y,
            depth + 30
        );
    }
}
