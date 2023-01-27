using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudInfo : MonoBehaviour
{
    [SerializeField]
    private float depth = 1;

    [SerializeField]
    private float speed = 1;
    private Vector3 origialSpawnPos;
    private float spawnTime;

    private void Update()
    {
        Transform cameraTransform = Camera.allCameras[0].transform;
        float distanceFromCamera = cameraTransform.position.y - origialSpawnPos.y;
        transform.position =
            origialSpawnPos
            + new Vector3(0.1f * speed * Time.time - spawnTime, distanceFromCamera / depth, 0);
    }

    private void Awake()
    {
        origialSpawnPos = transform.position;
        spawnTime = Time.time;
    }
}
