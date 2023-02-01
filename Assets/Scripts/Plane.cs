using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField]
    Vector2 startingVelocity;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = startingVelocity;
        originalSpawn = transform.position;
    }

    // Update is called once per frame
    [SerializeField]
    private float depth = 1;
    private Vector3 originalSpawn;

    // Update is called once per frame
    void Update()
    {
        Transform cameraTransform = Camera.allCameras[0].transform;
        float distanceFromCamera = cameraTransform.position.y;
        transform.position = new Vector3(
            transform.position.x,
            originalSpawn.y - distanceFromCamera / depth + cameraTransform.position.y,
            originalSpawn.z
        );
    }
}
