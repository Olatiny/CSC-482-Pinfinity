using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    public float depth = 1;
    private Vector3 originalSpawn;
    private float originalOffset;

    // Start is called before the first frame update
    void Awake()
    {
        originalSpawn = transform.position;
        originalOffset = transform.position.y - Camera.main.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Transform cameraTransform = Camera.main.transform;
        float cameraY = cameraTransform.position.y;
        transform.position = new Vector3(
            transform.position.x,
            ((originalSpawn.y - cameraY) / depth) + cameraY + originalOffset,
            1.0f
        );
        if (transform.position.y < cameraTransform.position.y - 10)
        {
            Destroy(gameObject);
        }
    }
}
