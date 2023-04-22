using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipBackground : MonoBehaviour
{
    public float depth;
    private Vector3 originalSpawn;
    private float originalOffset;
    private float velocityX;
    private float velocityY;

    // Start is called before the first frame update
    void Start()
    {
        float angle = Random.Range(0, 180);
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        velocityX = Mathf.Cos(angle * Mathf.Deg2Rad) / 3.0f;
        velocityY = Mathf.Sin(angle * Mathf.Deg2Rad) / 3.0f;
        originalSpawn = transform.position;
        originalOffset = transform.position.y - Camera.main.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        originalOffset += (velocityY * Time.deltaTime) / depth;
        transform.position += new Vector3((velocityX * Time.deltaTime) / depth, 0, 0);
        Transform cameraTransform = Camera.main.transform;
        float cameraY = cameraTransform.position.y;
        transform.position = new Vector3(
            transform.position.x,
            ((originalSpawn.y - cameraY) / depth) + cameraY + originalOffset,
            originalSpawn.z
        );
        if (
            transform.position.y < cameraTransform.position.y - 10
            || transform.position.x < -5
            || transform.position.x > 5
            || transform.position.y > cameraTransform.position.y + 20
        )
        {
            Destroy(gameObject);
        }
    }
}
