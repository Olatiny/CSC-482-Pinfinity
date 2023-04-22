using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField]
    Vector2 startingVelocity;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Rigidbody2D>().velocity = startingVelocity;
        originalSpawn = transform.position;
        GameManager.Instance.soundManager.FXPlaneFlyBy();
    }

    // Update is called once per frame
    [SerializeField]
    private float depth = 1;
    private Vector3 originalSpawn;

    // Update is called once per frame
    void Update()
    {
        Transform cameraTransform = Camera.main.transform;
        float distanceFromCamera = cameraTransform.position.y;
        transform.position = new Vector3(
            transform.position.x,
            originalSpawn.y - distanceFromCamera / depth + cameraTransform.position.y,
            originalSpawn.z
        );
        if (transform.position.y < Camera.main.transform.position.y - 5)
        {
            Destroy(gameObject);
        }
    }
}
